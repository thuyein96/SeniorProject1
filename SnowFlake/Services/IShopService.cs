using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Product;
using SnowFlake.Dtos.APIs.Shop.CreateShop;

namespace SnowFlake.Services;

public interface IShopService
{
    Task<ShopEntity> CreateAsync(CreateShopRequest createShopRequest);
    Task<ShopEntity> GetShopByHostRoomCode(string hostRoomCode);
    Task<bool> AddShopTokens(ShopEntity shop, int totalCost);
    //Task<bool> UpdateShopStock(ShopEntity shop, BuyProduct productEntity);

}
