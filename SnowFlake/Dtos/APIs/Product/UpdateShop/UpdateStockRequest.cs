namespace SnowFlake.Dtos.APIs.Product.UpdateShop;

public class UpdateStockRequest
{
    public string HostRoomCode { get; set; }
    public int TeamNumber { get; set; }
    public List<BuyProduct> Products { get; set; }
}
