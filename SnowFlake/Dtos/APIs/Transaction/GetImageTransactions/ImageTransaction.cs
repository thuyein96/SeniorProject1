namespace SnowFlake.Dtos.APIs.Transaction.GetImageTransactions;

public class ImageTransaction
{
    public int RoundNumber { get; set; }
    public string TeamId { get; set; }
    public string ShopId { get; set; }
    public string? ImageId { get; set; }
    public string? ImageName { get; set; }
    public int Quantity { get; set; }
    public int Total { get; set; }
}