using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Transaction.CreateTransaction;

namespace SnowFlake.Services;

public interface ITransactionService
{
    Task<TransactionEntity> CreateTransaction(CreateTransactionRequest createTransactionRequest);
    Task<List<TransactionEntity>> GetTransactions(int roundNumber, string shopId);
    Task<List<TransactionEntity>> GetTransactionsByTeamId(string teamId);
    Task<List<TransactionEntity>> GetImageTransactions(int roundNumber, string shopId);
    Task<List<TransactionEntity>> GetProductTransactions(int roundNumber, string shopId);
}