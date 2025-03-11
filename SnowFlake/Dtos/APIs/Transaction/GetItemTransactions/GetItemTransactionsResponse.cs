namespace SnowFlake.Dtos.APIs.Transaction.GetItemTransactions;

public class GetItemTransactionsResponse : BaseResponse
{
    public List<ItemTransaction> Message { get; set; }
}