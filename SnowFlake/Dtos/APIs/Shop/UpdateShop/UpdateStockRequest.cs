namespace SnowFlake.Dtos.APIs.Shop.UpdateShop;

public class UpdateStockRequest
{
    public string HostRoomCode { get; set; }
    public string SoldProduct { get; set; }
    public int Quantity { get; set; }
    public int TeamNumber { get; set; }

}
