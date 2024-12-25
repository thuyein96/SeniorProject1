using Microsoft.AspNetCore.Mvc;
using SnowFlake.Dtos.APIs.Shop.CreateShop;
using SnowFlake.Dtos.APIs.Shop.GetShop;
using SnowFlake.Dtos.APIs.Shop.UpdateShop;
using SnowFlake.Services;

namespace SnowFlake.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateShopRequest createShopRequest)
        {
            try
            {
                if (createShopRequest == null)
                {
                    return BadRequest();
                }
                var shop = await _shopService.CreateAsync(createShopRequest);
                if (shop == null)
                {
                    return NotFound(new CreateShopResponse
                    {
                        Success = false,
                        Message = null
                    });
                }
                return Ok(new CreateShopResponse
                {
                    Success = true,
                    Message = shop
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetShopByHostRoomCode(string hostRoomCode)
        {
            try
            {
                var shop = await _shopService.GetShopByHostRoomCodeAsync(hostRoomCode);
                if (shop == null)
                {
                    return NotFound(new GetShopResponse
                    {
                        Success = false,
                        Message = null
                    });
                }
                return Ok(new GetShopResponse
                {
                    Success = true,
                    Message = shop
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStock(UpdateStockRequest updateStockRequest)
        {
            try
            {
                if (updateStockRequest == null)
                {
                    return BadRequest();
                }
                var shop = await _shopService.UpdateStockAsync(updateStockRequest);
                if (shop == null)
                {
                    return NotFound(new UpdateStockResponse
                    {
                        Success = false,
                        Message = null
                    });
                }
                return Ok(new UpdateStockResponse
                {
                    Success = true,
                    Message = shop
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}
