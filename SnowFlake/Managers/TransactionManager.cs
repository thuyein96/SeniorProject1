using SnowFlake.Dtos;
using SnowFlake.Services;

namespace SnowFlake.Managers;

public class TransactionManager : ITransactionManager
{
    private readonly ITransactionService _transactionService;
    private readonly IShopService _shopService;
    private readonly ITeamService _teamService;

    public TransactionManager(ITransactionService transactionService,
                              IShopService shopService,
                              ITeamService teamService)
    {
        _transactionService = transactionService;
        _shopService = shopService;
        _teamService = teamService;
    }

    public async Task<List<TransactionEntity>> GetTransactionsWithShop(string hostRoomCode, string playerRoomCode, int roundNumber, int? teamNumber)
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

        var team = new TeamEntity();
        if (teamNumber.HasValue)
        {
            team = await _teamService.GetTeam(teamNumber.Value, playerRoomCode, hostRoomCode);
            return await _transactionService.GetTeamTransactionsByRound(roundNumber, shop.Id, team.Id);
        }
        return await _transactionService.GetTransactions(roundNumber, shop.Id);
    }
}