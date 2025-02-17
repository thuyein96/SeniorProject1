namespace SnowFlake.Dtos.APIs.Shop.SellSnowFlake;

public class BuySnowflakeRequest
{
    public bool IsBuyingConfirm { get; set; }
    public string? HostRoomCode { get; set; }
    public string? PlayerRoomCode { get; set; }
    public int RoundNumber { get; set; }
    public int TeamNumber { get; set; }
    public string ImageUrl { get; set; }
    public int Price { get; set; }

}