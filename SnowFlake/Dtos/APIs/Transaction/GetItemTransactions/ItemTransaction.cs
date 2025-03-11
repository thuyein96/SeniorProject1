namespace SnowFlake.Dtos.APIs.Transaction.GetItemTransactions;

public class ItemTransaction
{
    public int RoundNumber { get; set; }
    public string TeamId { get; set; }
    public string ShopId { get; set; }
    public string? ItemId { get; set; }
    public string? ItemName { get; set; }
    public int Quantity { get; set; }
    public int Total { get; set; }
}