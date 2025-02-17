using MongoDB.Bson;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;
using SnowFlake.Azure.BlobsStorageService;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Image.CreateImage;
using SnowFlake.Dtos.APIs.Image.DeleteImage;
using SnowFlake.Dtos.APIs.Image.GetImage;
using SnowFlake.Dtos.APIs.Image.UpdateImage;
using SnowFlake.UnitOfWork;
using SnowFlake.Utilities;

namespace SnowFlake.Services;

public class ImageService : IImageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobStorageService _blobStorageService;

    public ImageService(IUnitOfWork unitOfWork, IBlobStorageService blobStorageService)
    {
        _unitOfWork = unitOfWork;
        _blobStorageService = blobStorageService;
    }

    public async Task<ImageEntity> AddImage(CreateImageRequest createImageRequest, IFormFile file)
    {
        var readStream = file.OpenReadStream();
        var imageUploadUrl = await _blobStorageService.UploadBlobAsync("image", file.FileName, readStream);

        var imageEntity = new ImageEntity
        {
            Id = ObjectId.GenerateNewId().ToString(),
            FileName = file.FileName,
            SnowFlakeImageUrl = imageUploadUrl,
            ImageBuyingStatus = ImageBuyingStatus.Pending.Name,
            OwnerId = createImageRequest.TeamId,
            CreationDate = DateTime.Now
        };

        await _unitOfWork.ImageRepository.Create(imageEntity);
        await _unitOfWork.Commit();

        return imageEntity;
    }

    public async Task<List<ImageEntity>> GetImages()
    {
        var images = (await _unitOfWork.ImageRepository.GetAll()).Take(Utils.BatchSize).ToList();
        return images;
    }

    public async Task<List<ImageEntity>> GetImagesByOwnerId(string ownerId)
    {
        var images = (await _unitOfWork.ImageRepository.GetBy(i => i.OwnerId == ownerId)).ToList();
        return images;
    }

    public async Task<ImageEntity> GetImage(GetImageRequest getImageRequest)
    {
        if (string.IsNullOrWhiteSpace(getImageRequest.Id))
        {
            return null;
        }
        var image = (await _unitOfWork.ImageRepository.GetBy(i => i.Id == getImageRequest.Id)).FirstOrDefault();

        if (image == null)
        {
            return null;
        }

        return image;
    }

    public async Task<string> DeleteImage(DeleteImageRequest deleteImageRequest)
    {
        if (string.IsNullOrWhiteSpace(deleteImageRequest.Id) || string.IsNullOrWhiteSpace(deleteImageRequest.ContainerName) || string.IsNullOrWhiteSpace(deleteImageRequest.BlobName))
        {
            return string.Empty;
        }

        var isDeleted = await _blobStorageService.DeleteBlobAsync(deleteImageRequest.ContainerName, deleteImageRequest.BlobName);

        if (!isDeleted)
        {
            return string.Empty;
        }


        var existingImage = (await _unitOfWork.ImageRepository.GetBy(i => i.Id == deleteImageRequest.Id)).FirstOrDefault();

        if (existingImage is null)
        {
            return string.Empty;
        }

        await _unitOfWork.ImageRepository.Delete(existingImage);

        return $"[ID: {deleteImageRequest.Id}] Successfully Deleted";
    }

    public async Task<ImageEntity> UpdateImage(ImageEntity image)
    {
        if (image is null) return null;
        await _unitOfWork.ImageRepository.Update(image);
        await _unitOfWork.Commit();

        return image;
    }

    public async Task<string> UpdateImage(UpdateImageRequest updateImageRequest)
    {
        var existingImage = (await _unitOfWork.ImageRepository.GetBy(i => i.Id == updateImageRequest.Id)).FirstOrDefault();

        // Override with new image
        if (!string.IsNullOrWhiteSpace(updateImageRequest.NewImageFileName) && (updateImageRequest.NewImageByteData != null))
        {
            if (string.IsNullOrWhiteSpace(updateImageRequest.OldImageFileName))
            {
                return null;
            }
            var isDeleted = await _blobStorageService.DeleteBlobAsync("images", updateImageRequest.OldImageFileName);

            if (!isDeleted)
            {
                return null;
            }

            var imageUploadUrl = await _blobStorageService.UploadBlobAsync("images", updateImageRequest.NewImageFileName, updateImageRequest.NewImageByteData);

            existingImage.SnowFlakeImageUrl = imageUploadUrl;
        }

        // Update Image Price
        if (updateImageRequest.Price > 0)
        {
            existingImage.Price = updateImageRequest.Price;
        }

        if (!string.IsNullOrWhiteSpace(updateImageRequest.ImageBuyingStatus))
        {
            existingImage.ImageBuyingStatus = updateImageRequest.ImageBuyingStatus;
        }

        existingImage.ModifiedDate = DateTime.Now;

        await _unitOfWork.ImageRepository.Update(existingImage);
        await _unitOfWork.Commit();

        return $"[ID: {existingImage.Id}] Successfully Updated";
    }


}
