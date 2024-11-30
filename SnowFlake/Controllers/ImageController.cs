using Microsoft.AspNetCore.Mvc;
using SnowFlake.Azure.BlobsStorageService;
using SnowFlake.Dtos.APIs.Image.UploadImage;

namespace SnowFlake.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IBlobStorageService _blobStorageService;

        public ImageController(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new UploadImageResponse
                {
                    Success = false,
                    Message = "File is empty"
                });
            }

            using var stream = file.OpenReadStream();
            var imageurl = await _blobStorageService.UploadBlobAsync("images", file.FileName, stream);

            return Ok(new UploadImageResponse
            {
                Success = true,
                Message = imageurl
            });
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteImage(string imageName)
        {
            if (string.IsNullOrWhiteSpace(imageName))
            {
                return BadRequest(new UploadImageResponse
                {
                    Success = false,
                    Message = "Image name is empty"
                });
            }
            var result = await _blobStorageService.DeleteBlobAsync("images", imageName);
            if (!result)
            {
                return NotFound(new UploadImageResponse
                {
                    Success = false,
                    Message = "Image not found"
                });
            }
            return Ok(new UploadImageResponse
            {
                Success = true,
                Message = "Image deleted"
            });
        }
    }
}
