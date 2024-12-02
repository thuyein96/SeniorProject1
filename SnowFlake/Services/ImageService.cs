using SnowFlake.Azure.BlobsStorageService;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Image.CreateImage;
using SnowFlake.Dtos.APIs.Image.UpdateImage;
using SnowFlake.Dtos.APIs.Team.UpdateTeam;
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

    public async Task<ImageEntity> AddImage(CreateImageRequest createImageRequest)
    {
        var imageUploadUrl = await _blobStorageService.UploadBlobAsync("image", createImageRequest.FileName, createImageRequest.ImageByteData);

        var imageEntity = new ImageEntity
        {
            Id = createImageRequest.Id.ToString(),
            SnowFlakeImageUrl = imageUploadUrl,
            ImageBuyingStatus = ImageBuyingStatus.Pending.Name,
            TeamId = createImageRequest.TeamId,
            CreationDate = DateTime.Now
        };

        _unitOfWork.ImageRepository.Create(imageEntity);
        _unitOfWork.Commit();

        return imageEntity;
    }

    public Task<string> DeleteImage(string id, string containerName, string BlogName)
    {
        throw new NotImplementedException();
    }

    public async Task<string> UpdateImage(UpdateImageRequest updateImageRequest)
    {
        var existingImage = (await _unitOfWork.ImageRepository.GetBy(i => i.Id == updateImageRequest.Id)).FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(updateImageRequest.NewImageFileName) &&  (updateImageRequest.NewImageByteData != null))
        {
            if(string.IsNullOrWhiteSpace(updateImageRequest.OldImageFileName))
            {
                return null;
            }
            var isDeleted = await _blobStorageService.DeleteBlobAsync("images", updateImageRequest.OldImageFileName);

            if(!isDeleted)
            {
                return null;
            }

            var imageUploadUrl = await _blobStorageService.UploadBlobAsync("images", updateImageRequest.NewImageFileName, updateImageRequest.NewImageByteData);

            existingImage.SnowFlakeImageUrl = imageUploadUrl;
        }

        if(!string.IsNullOrWhiteSpace(updateImageRequest.ImageBuyingStatus))
        {
            existingImage.ImageBuyingStatus = updateImageRequest.ImageBuyingStatus;
        }

        existingImage.ModifiedDate = DateTime.Now;

        _unitOfWork.ImageRepository.Create(existingImage);
        _unitOfWork.Commit();

        return $"[ID: {existingImage.Id}] Successfully Updated";
    }
}
