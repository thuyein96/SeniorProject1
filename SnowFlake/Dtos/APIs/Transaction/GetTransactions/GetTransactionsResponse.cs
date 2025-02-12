namespace SnowFlake.Dtos.APIs.Transaction.GetTransactions;

public class GetTransactionsResponse : BaseResponse
{
    public List<TransactionEntity> Message { get; set; }
}