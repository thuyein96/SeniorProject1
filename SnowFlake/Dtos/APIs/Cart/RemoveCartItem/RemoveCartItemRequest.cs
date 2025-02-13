namespace SnowFlake.Dtos.APIs.Cart.RemoveCartItem;

public class RemoveCartItemRequest
{
    public string? HostRoomCode { get; set; }
    public string? PlayerRoomCode { get; set; }
    public string ProductName { get; set; }
    public int TeamNumber { get; set; }
}