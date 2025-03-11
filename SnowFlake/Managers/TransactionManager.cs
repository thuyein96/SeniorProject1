using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Transaction.CreateTransaction;
using SnowFlake.Dtos.APIs.Transaction.GetImageTransactions;
using SnowFlake.Dtos.APIs.Transaction.GetItemTransactions;
using SnowFlake.Dtos.APIs.Transaction.GetTransactions;
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

    public async Task<CreateTransactionResponse> CreateTransaction(CreateTransactionRequest createTransactionRequest)
    {
        var transaction = new TransactionEntity
        {
            Id = ObjectId.GenerateNewId().ToString(),
            RoundNumber = createTransactionRequest.RoundNumber,
            ShopId = createTransactionRequest.ShopId,
            TeamId = createTransactionRequest.TeamId,
            Quantity = createTransactionRequest.Quantity,
            Total = createTransactionRequest.Total,
            CreationDate = DateTime.Now,
            ModifiedDate = null
        };

        if (!string.IsNullOrWhiteSpace(createTransactionRequest.ImageId))
        {
            transaction.ImageId = createTransactionRequest.ImageId;
            transaction.ImageName = createTransactionRequest.ImageName;
        }

        if (!string.IsNullOrWhiteSpace(createTransactionRequest.ProductId))
        {
            transaction.ProductId = createTransactionRequest.ProductId;
            transaction.ProductName = createTransactionRequest.ProductName;
        }

        var transactionResult = await _transactionService.CreateTransaction(transaction);
        if (transactionResult is null) return new CreateTransactionResponse
        {
            Success = false,
            Message = null
        };
        return new CreateTransactionResponse
        {
            Success = true,
            Message = transactionResult
        };
    }
    public async Task<GetTransactionsResponse> GetTransactionsWithShop(string hostRoomCode, string playerRoomCode, int roundNumber, int? teamNumber)
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
            return new GetTransactionsResponse
            {
                Success = false,
                Message = null
            };
        }

        var team = new TeamEntity();
        var transactions = new List<TransactionEntity>();
        if (teamNumber.HasValue)
        {
            team = await _teamService.GetTeam(teamNumber.Value, playerRoomCode, hostRoomCode);
            transactions = await _transactionService.GetTeamTransactionsByRound(roundNumber, shop.Id, team.Id);
        }
        else
        {
            transactions = await _transactionService.GetTransactions(roundNumber, shop.Id);
        }

        return transactions is not null ? 
            new GetTransactionsResponse
        {
            Success = true,
            Message = transactions
        } : new GetTransactionsResponse
        {
            Success = false,
            Message = null
        };
    }

    public async Task<GetItemTransactionsResponse> GetItemTransactionsWithShop(string hostRoomCode, string playerRoomCode, int roundNumber, int? teamNumber)
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
            return new GetItemTransactionsResponse
            {
                Success = false,
                Message = null
            };
        }

        var team = new TeamEntity();
        var transactionResult = new List<TransactionEntity>();
        if (teamNumber.HasValue)
        {
            team = await _teamService.GetTeam(teamNumber.Value, playerRoomCode, hostRoomCode);
            transactionResult = await _transactionService.GetTeamTransactionsByRound(roundNumber, shop.Id, team.Id);
        }
        else
        {
            transactionResult = await _transactionService.GetTransactions(roundNumber, shop.Id);
        }

        var transactions = transactionResult.Where(s => s.ImageId == null).Select(i => new ItemTransaction
        {
            RoundNumber = i.RoundNumber,
            ShopId = i.ShopId,
            TeamId = i.TeamId,
            ItemId = i.ProductId,
            ItemName = i.ProductName,
            Quantity = i.Quantity,
            Total = i.Total

        }).ToList();

        return transactions is not null ?
            new GetItemTransactionsResponse
            {
                Success = true,
                Message = transactions
            } : new GetItemTransactionsResponse
            {
                Success = false,
                Message = null
            };
    }

    public async Task<GetImageTransactionsResponse> GetImageTransactionsWithShop(string hostRoomCode, string playerRoomCode, int roundNumber, int? teamNumber)
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
            return new GetImageTransactionsResponse
            {
                Success = false,
                Message = null
            };
        }

        var team = new TeamEntity();
        var transactionResult = new List<TransactionEntity>();
        if (teamNumber.HasValue)
        {
            team = await _teamService.GetTeam(teamNumber.Value, playerRoomCode, hostRoomCode);
            transactionResult = await _transactionService.GetTeamTransactionsByRound(roundNumber, shop.Id, team.Id);
        }
        else
        {
            transactionResult = await _transactionService.GetTransactions(roundNumber, shop.Id);
        }

        var transactions = transactionResult.Where(s => s.ProductId == null).Select(i => new ImageTransaction
        {
            RoundNumber = i.RoundNumber,
            ShopId = i.ShopId,
            TeamId = i.TeamId,
            ImageId = i.ImageId,
            ImageName = i.ImageName,
            Quantity = i.Quantity,
            Total = i.Total

        }).ToList();

        return transactions is not null ?
            new GetImageTransactionsResponse
            {
                Success = true,
                Message = transactions
            } : new GetImageTransactionsResponse
            {
                Success = false,
                Message = null
            };
    }
}