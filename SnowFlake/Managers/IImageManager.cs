using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Image.DeleteImage;

namespace SnowFlake.Managers;

public interface IImageManager
{
    Task<DeleteImageResponse> DeleteImage(DeleteImageRequest deleteImageRequest);
}