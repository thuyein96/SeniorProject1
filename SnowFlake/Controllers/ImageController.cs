using Microsoft.AspNetCore.Mvc;
using SnowFlake.Azure.BlobsStorageService;
using SnowFlake.Dtos.APIs.Image.CreateImage;
using SnowFlake.Dtos.APIs.Image.DeleteImage;
using SnowFlake.Dtos.APIs.Image.UpdateImage;
using SnowFlake.Dtos.APIs.Image.UploadImage;
using SnowFlake.Managers;
using SnowFlake.Services;

namespace SnowFlake.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IImageManager _imageManager;

        public ImageController(IImageService imageService,
                               IImageManager imageManager)
        {
            _imageService = imageService;
            _imageManager = imageManager;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromQuery] string teamId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(teamId))
                {
                    return BadRequest(new CreateImageResponse
                    {
                        Success = false,
                        Message = null
                    });
                }

                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();

                var image = await _imageService.AddImage(new CreateImageRequest { TeamId = teamId }, file);

                if (image is null)
                {
                    return NotFound(new CreateImageResponse
                    {
                        Success = false,
                        Message = null
                    });
                }

                return Ok(new CreateImageResponse
                {
                    Success = true,
                    Message = image
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateImage(UpdateImageRequest updateImageRequest)
        {
            if (updateImageRequest is null)
            {
                return BadRequest(new UploadImageResponse
                {
                    Success = false,
                    Message = "Request body is not valid"
                });
            }
            var result = await _imageService.UpdateImage(updateImageRequest);
            if (string.IsNullOrWhiteSpace(result))
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
                Message = result
            });
        }

        [HttpDelete("{imageid}")]
        public async Task<IActionResult> DeleteImage(string imageId)
        {
            if (string.IsNullOrWhiteSpace(imageId))
            {
                return BadRequest(new UploadImageResponse
                {
                    Success = false,
                    Message = "Request body is not valid"
                });
            }
            var deleteResult = await _imageManager.DeleteImage(new DeleteImageRequest{ ImageId = imageId });

            return deleteResult.Success ? Ok(deleteResult) : NotFound(deleteResult);
        }
    }
}
