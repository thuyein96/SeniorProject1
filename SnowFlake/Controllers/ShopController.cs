using Microsoft.AspNetCore.Mvc;
using SnowFlake.Dtos.APIs.Product.UpdateShop;
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
                var shop = await _shopManager.CreateShop(createShopRequest);

                return shop.Success ? Ok(shop) : NotFound(shop);
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

                return shop.Success ? Ok(shop) : NotFound(shop);
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

                return orderResult.Success ? Ok(orderResult) : NotFound(orderResult);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("buyimage")]
        public async Task<IActionResult> ManageSnowFlakeOrder(BuySnowflakeRequest buySnowflakeRequest)
        {
            try
            {
                if (buySnowflakeRequest == null)
                {
                    return BadRequest();
                }
                var orderResult = await _shopManager.ManageSnowflakeOrder(buySnowflakeRequest);

                return orderResult.Success ? Ok(orderResult) : NotFound(orderResult);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("stock")]
        public async Task<IActionResult> UpdateShopStock(UpdateStockRequest updateStockRequest)
        {
            try
            {
                if (updateStockRequest == null)
                {
                    return BadRequest();
                }
                var orderResult = await _shopManager.AddProductsToShop(updateStockRequest);
                return orderResult.Success ? Ok(orderResult) : NotFound(orderResult);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
