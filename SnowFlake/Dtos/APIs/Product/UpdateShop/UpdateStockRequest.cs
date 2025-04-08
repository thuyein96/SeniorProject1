namespace SnowFlake.Dtos.APIs.Product.UpdateShop;

public class UpdateStockRequest
{
    public string HostRoomCode { get; set; }
    public string ProductName { get; set; }
    public int QuantityToAdd { get; set; }
}
