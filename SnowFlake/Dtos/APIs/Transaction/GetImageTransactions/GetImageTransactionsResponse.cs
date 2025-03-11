namespace SnowFlake.Dtos.APIs.Transaction.GetImageTransactions;

public class GetImageTransactionsResponse : BaseResponse
{
    public List<ImageTransaction> Message { get; set; }
}