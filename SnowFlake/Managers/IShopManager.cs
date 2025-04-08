using SnowFlake.Dtos.APIs.Product.UpdateShop;
using SnowFlake.Dtos.APIs.Shop.BuySnowFlake;
using SnowFlake.Dtos.APIs.Shop.CreateShop;
using SnowFlake.Dtos.APIs.Shop.GetShop;
using SnowFlake.Dtos.APIs.Shop.SellSnowFlake;
using SnowFlake.Dtos.APIs.Shop.UpdateShop;

namespace SnowFlake.Managers;

public interface IShopManager
{
    Task<CreateShopResponse> CreateShop(CreateShopRequest createShopRequest);
    Task<ExchangeProductsResponse> ManageIncomingShopOrder(ExchangeProductsRequest updateShopStockRequest);
    Task<GetShopResponse> GetShopByHostRoomCode(string hostRoomCode);
    Task<BuySnowflakeResponse> ManageSnowflakeOrder(BuySnowflakeRequest buySnowflakeRequest);
    Task<UpdateStockResponse> AddProductsToShop(UpdateStockRequest updateStockRequest);
}
