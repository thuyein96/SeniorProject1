using SnowFlake.Dtos.APIs.Image.CreateImage;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Image.UpdateImage;

namespace SnowFlake.Services;

public interface IImageService
{
    Task<ImageEntity> AddImage(CreateImageRequest createImageRequest);
    Task<string> UpdateImage(UpdateImageRequest updateImageRequest);
    Task<string> DeleteImage(string id, string containerName, string BlogName);
}
