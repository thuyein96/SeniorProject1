using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Transaction.GetTransactions;

namespace SnowFlake.Managers;

public interface ITransactionManager
{
    Task<GetTransactionsResponse> GetTransactionsWithShop(string hostRoomCode, string playerRoomCode, int roundNumber, int? teamNumber);
}