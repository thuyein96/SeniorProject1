namespace SnowFlake.Dtos;

public class TransactionEntity : BaseEntity
{
    public int RoundNumber { get; set; }
    public string TeamId { get; set; }
    public string ShopId { get; set; }
    public string? ProductId { get; set; }
    public string? ImageId { get; set; }
    public int Quantity { get; set; }
    public int Total { get; set; }
}