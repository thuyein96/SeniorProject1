namespace SnowFlake.Dtos.APIs.Shop.CreateShop;

public class CreateShopRequest
{
    public string HostRoomCode { get; set; }
    public string PlayerRoomCode { get; set; }
    public int Tokens { get; set; }
    public List<Product> ShopStocks { get; set; }
}
