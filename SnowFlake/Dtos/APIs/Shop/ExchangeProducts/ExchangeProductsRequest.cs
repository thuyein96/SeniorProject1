using SnowFlake.Dtos.APIs.Product;

namespace SnowFlake.Dtos.APIs.Shop.UpdateShop;

public class ExchangeProductsRequest
{
    public string HostRoomCode { get; set; }
    public int TeamNumber { get; set; }
    public List<BuyProduct> Products { get; set; }
}
