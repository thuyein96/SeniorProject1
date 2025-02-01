using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Image.CreateImage;
using SnowFlake.Dtos.APIs.Image.DeleteImage;
using SnowFlake.Dtos.APIs.Image.GetImage;
using SnowFlake.Dtos.APIs.Image.UpdateImage;

namespace SnowFlake.Services;

public interface IImageService
{
    Task<ImageEntity> UploadImage(UploadImageRequest uploadImageRequest);
    Task<ImageEntity> GetImage(GetImageRequest getImageRequest);
    Task<List<ImageEntity>> GetImages();
    Task<bool> UpdateImageOwner(ImageEntity image);
    Task<string> UpdateImage(UpdateImageRequest updateImageRequest);
    Task<string> DeleteImage(DeleteImageRequest deleteImageRequest);
}
