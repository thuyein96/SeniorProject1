using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Transaction.GetImageTransactions;
using SnowFlake.Dtos.APIs.Transaction.GetItemTransactions;
using SnowFlake.Dtos.APIs.Transaction.GetTransactions;

namespace SnowFlake.Managers;

public interface ITransactionManager
{
    Task<GetTransactionsResponse> GetTransactionsWithShop(string hostRoomCode, string playerRoomCode, int roundNumber, int? teamNumber);

    Task<GetItemTransactionsResponse> GetItemTransactionsWithShop(string hostRoomCode, string playerRoomCode,
                                                              int roundNumber, int? teamNumber);

    Task<GetImageTransactionsResponse> GetImageTransactionsWithShop(string hostRoomCode, string playerRoomCode,
                                                               int roundNumber, int? teamNumber);
}