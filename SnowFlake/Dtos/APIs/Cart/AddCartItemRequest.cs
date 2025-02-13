namespace SnowFlake.Dtos.APIs.Cart;

public class AddCartItemRequest
{
    public string? HostRoomCode { get; set; }
    public string? PlayerRoomCode { get; set; }
    public string ProductName { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public int TeamNumber { get; set; }
}