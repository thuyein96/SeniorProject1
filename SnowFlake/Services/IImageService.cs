using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Image.CreateImage;
using SnowFlake.Dtos.APIs.Image.DeleteImage;
using SnowFlake.Dtos.APIs.Image.GetImage;
using SnowFlake.Dtos.APIs.Image.UpdateImage;

namespace SnowFlake.Services;

public interface IImageService
{
    Task<ImageEntity> AddImage(CreateImageRequest createImageRequest, IFormFile file);
    Task<ImageEntity> GetImage(GetImageRequest getImageRequest);
    Task<List<ImageEntity>> GetImages();
    Task<string> UpdateImage(UpdateImageRequest updateImageRequest);
    Task<ImageEntity> UpdateImage(ImageEntity image);
    Task<string> DeleteImage(DeleteImageRequest deleteImageRequest);
}
