using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Image.CreateImage;
using SnowFlake.Dtos.APIs.Image.GetImage;
using SnowFlake.Dtos.APIs.Image.UpdateImage;

namespace SnowFlake.Services;

public interface IImageService
{
    Task<ImageEntity> AddImage(CreateImageRequest createImageRequest, IFormFile file);
    Task<ImageEntity> GetImage(GetImageRequest getImageRequest);
    Task<List<ImageEntity>> GetImagesByTeamId(string teamId, string imageBuyingStatus);
    Task<ImageEntity> GetImageByUrl(string imageUrl, string imageBuyingStatus);
    Task<string> UpdateImage(UpdateImageRequest updateImageRequest);
    Task<ImageEntity> UpdateImage(ImageEntity image);
    Task<bool> DeleteImageFromDb(ImageEntity image);
    Task<bool> DeleteImageFromBlob(string fileName);
}
