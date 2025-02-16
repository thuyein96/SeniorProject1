using SnowFlake.Dtos;

namespace SnowFlake.Managers;

public interface ITransactionManager
{
    Task<List<TransactionEntity>> GetTransactionsWithShop(string hostRoomCode, string playerRoomCode, int roundNumber, int? teamNumber);
}