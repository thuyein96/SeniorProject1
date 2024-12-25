using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Shop.CreateShop;
using SnowFlake.Dtos.APIs.Shop.UpdateShop;

namespace SnowFlake.Services;

public interface IShopService
{
    Task<ShopEntity> CreateAsync(CreateShopRequest createShopRequest);
    Task<ShopEntity> GetShopByHostRoomCodeAsync(string hostRoomCode);
    Task<string> UpdateStockAsync(UpdateStockRequest updateStockRequest);

}
