namespace SnowFlake.Dtos.APIs.Shop.SellSnowFlake;

public class BuySnowflakeRequest
{
    public string PlayerRoomCode { get; set; }
    public int TeamNumber { get; set; }
    public string ImageId { get; set; }
    public int Price { get; set; }

}