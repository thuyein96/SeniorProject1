//using Microsoft.AspNetCore.Mvc;
//using SnowFlake.Azure.BlobsStorageService;
//using SnowFlake.Dtos.APIs.Image.CreateImage;
//using SnowFlake.Dtos.APIs.Image.DeleteImage;
//using SnowFlake.Dtos.APIs.Image.UpdateImage;
//using SnowFlake.Dtos.APIs.Image.UploadImage;
//using SnowFlake.Services;
//using UploadImageRequest = SnowFlake.Dtos.APIs.Image.CreateImage.UploadImageRequest;

//namespace SnowFlake.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ImageController : ControllerBase
//    {
//        private readonly IImageService _imageService;

//        public ImageController(IImageService imageService)
//        {
//            _imageService = imageService;
//        }

//        [HttpPost]
//        public async Task<IActionResult> UploadImage(UploadImageRequest uploadImageRequest)
//        {
//            try
//            {
//                if (uploadImageRequest is null)
//                {
//                    return BadRequest(new CreateImageResponse
//                    {
//                        Success = false,
//                        Message = null
//                    });
//                }

//                var image = await _imageService.UploadImage(uploadImageRequest);

//                if (image is null)
//                {
//                    return NotFound(new CreateImageResponse
//                    {
//                        Success = false,
//                        Message = null
//                    });
//                }

//                return Ok(new CreateImageResponse
//                {
//                    Success = true,
//                    Message = image
//                });
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500, e);
//            }
//        }

//        [HttpPut]
//        public async Task<IActionResult> UpdateImage(UpdateImageRequest updateImageRequest)
//        {
//            if (updateImageRequest is null)
//            {
//                return BadRequest(new UploadImageResponse
//                {
//                    Success = false,
//                    Message = "Request body is not valid"
//                });
//            }
//            var result = await _imageService.UpdateImage(updateImageRequest);
//            if (string.IsNullOrWhiteSpace(result))
//            {
//                return NotFound(new UploadImageResponse
//                {
//                    Success = false,
//                    Message = "Image not found"
//                });
//            }
//            return Ok(new UploadImageResponse
//            {
//                Success = true,
//                Message = result
//            });
//        }

//        [HttpDelete]
//        public async Task<IActionResult> DeleteImage(DeleteImageRequest deleteImageRequest)
//        {
//            if (deleteImageRequest is null)
//            {
//                return BadRequest(new UploadImageResponse
//                {
//                    Success = false,
//                    Message = "Request body is not valid"
//                });
//            }
//            var result = await _imageService.DeleteImage(deleteImageRequest);

//            if (string.IsNullOrWhiteSpace(result))
//            {
//                return NotFound(new DeleteImageResponse
//                {
//                    Success = false,
//                    Message = "Image not found"
//                });
//            }
//            return Ok(new DeleteImageResponse
//            {
//                Success = true,
//                Message = result
//            });
//        }
//    }
//}
