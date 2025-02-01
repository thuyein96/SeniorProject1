namespace SnowFlake.Dtos.APIs.Shop.GetShop;

public class ShopWithProducts
{
    public string Id { get; set; }
    public string HostRoomCode { get; set; }
    public string PlayerRoomCode { get; set; }
    public int Tokens { get; set; }
    public List<Dtos.Product> ShopStocks { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}