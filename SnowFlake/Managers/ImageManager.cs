using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Image.DeleteImage;
using SnowFlake.Dtos.APIs.Image.GetImage;
using SnowFlake.Dtos.APIs.Team.DeleteTeam;
using SnowFlake.Services;

namespace SnowFlake.Managers;

public class ImageManager : IImageManager
{
    private readonly IImageService _imageService;

    public ImageManager(IImageService imageService)
    {
        _imageService = imageService;
    }

    public async Task<DeleteImageResponse> DeleteImage(DeleteImageRequest deleteImageRequest)
    {
        if (string.IsNullOrWhiteSpace(deleteImageRequest.ImageId))
            return new DeleteImageResponse
            {
                Success = false,
                Message = "ImageUrl is required"
            };

        var image = await _imageService.GetImage(new GetImageRequest{ ImageId = deleteImageRequest.ImageId });

        return await DeleteImage(image) 
            ? new DeleteImageResponse
            {
                Success = true,
                Message = "Image deleted successfully"
            } : new DeleteImageResponse
            {
                Success = false,
                Message = "Failed to delete image"
            };
    }

    private async Task<bool> DeleteImage(ImageEntity image)
    {
        var deleteImageEntity = await _imageService.DeleteImageFromDb(image);
        var deletedImageBlob = await _imageService.DeleteImageFromBlob(image.FileName);
        return deleteImageEntity && deletedImageBlob;
    }
}