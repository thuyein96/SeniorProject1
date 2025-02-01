using Microsoft.AspNetCore.Mvc;
using SnowFlake.Dtos.APIs.Shop.BuySnowFlake;
using SnowFlake.Dtos.APIs.Shop.CreateShop;
using SnowFlake.Dtos.APIs.Shop.GetShop;
using SnowFlake.Dtos.APIs.Shop.SellSnowFlake;
using SnowFlake.Dtos.APIs.Shop.UpdateShop;
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
                var shop = await _shopManager.GetShopByHostRoomCode(hostRoomCode);
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

        [HttpPut("exchangestocks")]
        public async Task<IActionResult> ManageShopOrder(ExchangeProductsRequest exchangeProductsRequest)
        {
            try
            {
                if (exchangeProductsRequest == null)
                {
                    return BadRequest();
                }
                var orderResult = await _shopManager.ManageIncomingShopOrder(exchangeProductsRequest);
                if (orderResult == null)
                {
                    return NotFound(new ExchangeProductsResponse
                    {
                        Success = false,
                        Message = null
                    });
                }
                return Ok(new ExchangeProductsResponse
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

        //[HttpPut("buyimage")]
        //public async Task<IActionResult> ManageSnowFlakeOrder(BuySnowflakeRequest buySnowflakeRequest)
        //{
        //    try
        //    {
        //        if (buySnowflakeRequest == null)
        //        {
        //            return BadRequest();
        //        }
        //        var orderResult = ;
        //        if (orderResult == null)
        //        {
        //            return NotFound(new BuySnowflakeResponse
        //            {
        //                Success = false,
        //                Message = null
        //            });
        //        }
        //        return Ok(new BuySnowflakeResponse
        //        {
        //            Success = true,
        //            Message = orderResult
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(500, e.Message);
        //    }
        //}
    }
}
