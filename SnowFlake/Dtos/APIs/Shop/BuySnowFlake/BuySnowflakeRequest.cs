namespace SnowFlake.Dtos.APIs.Shop.SellSnowFlake;

public class BuySnowflakeRequest
{
    public bool IsBuyingConfirm { get; set; }
    public string PlayerRoomCode { get; set; }
    public int RoundNumber { get; set; }
    public int TeamNumber { get; set; }
    public string ImageId { get; set; }
    public int Price { get; set; }

}