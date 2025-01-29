using SnowFlake.Dtos.APIs.Product.UpdateShop;

namespace SnowFlake.Managers;

public interface IShopManager
{
    Task<string> ManageIncomingShopOrder(UpdateStockRequest updateShopStockRequest);
}
