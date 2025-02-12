using SnowFlake.Dtos;
using SnowFlake.Services;

namespace SnowFlake.Managers;

public class TransactionManager : ITransactionManager
{
    private readonly ITransactionService _transactionService;
    private readonly IShopService _shopService;

    public TransactionManager(ITransactionService transactionService,
                              IShopService shopService)
    {
        _transactionService = transactionService;
        _shopService = shopService;
    }

    public async Task<List<TransactionEntity>> GetTransactionsWithShop(string hostRoomCode, string playerRoomCode, int roundNumber)
    {
        var shop = new ShopEntity();
        if (!string.IsNullOrWhiteSpace(hostRoomCode))
        {
            shop = await _shopService.GetShopByHostRoomCode(hostRoomCode);
        }

        if (!string.IsNullOrWhiteSpace(playerRoomCode))
        {
            shop = await _shopService.GetShopByPlayerRoomCode(playerRoomCode);
        }

        if (shop == null)
        {
            return null;
        }
        return await _transactionService.GetTransactions(roundNumber, shop.Id);
    }
}