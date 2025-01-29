using Microsoft.AspNetCore.Mvc;
using SnowFlake.Dtos.APIs.Product.UpdateShop;
using SnowFlake.Dtos.APIs.Shop.CreateShop;
using SnowFlake.Dtos.APIs.Shop.GetShop;
using SnowFlake.Managers;
using SnowFlake.Services;

namespace SnowFlake.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;
        private readonly IShopManager _shopManager;

        public ShopController(IShopService shopService,
                              IShopManager shopManager)
        {
            _shopService = shopService;
            _shopManager = shopManager;
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
                var shop = await _shopService.GetShopByHostRoomCode(hostRoomCode);
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
        public async Task<IActionResult> ManageShopOrder(UpdateStockRequest updateStockRequest)
        {
            try
            {
                if (updateStockRequest == null)
                {
                    return BadRequest();
                }
                var orderResult = await _shopManager.ManageIncomingShopOrder(updateStockRequest);
                if (orderResult == null)
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
                    Message = orderResult
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}
