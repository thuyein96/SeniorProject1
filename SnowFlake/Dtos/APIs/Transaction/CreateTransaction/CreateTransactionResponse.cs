namespace SnowFlake.Dtos.APIs.Transaction.CreateTransaction;

public class CreateTransactionResponse : BaseResponse
{
    public TransactionEntity Message { get; set; }
}