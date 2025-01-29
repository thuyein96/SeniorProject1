using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Playground;
using SnowFlake.Dtos.APIs.Playground.ConfigurePlayground;
using SnowFlake.Dtos.APIs.Shop.CreateShop;
using SnowFlake.Dtos.APIs.Team.CreateTeam;
using SnowFlake.Services;
using SnowFlake.Utilities;

namespace SnowFlake.Managers;

public class PlaygroundManger : IPlaygroundManager
{
    private readonly ITeamService _teamService;
    private readonly IShopService _shopService;
    private readonly IPlaygroundService _playgroundService;
    private readonly IProductService _productService;

    public PlaygroundManger(ITeamService teamService,
                           IPlaygroundService playgroundService,
                           IProductService productService,
                           IShopService shopService)
    {
        _teamService = teamService;
        _shopService = shopService;
        _playgroundService = playgroundService;
        _productService = productService;
    }

    public async Task<PlaygroundEntity> SetupPlayground(ConfigurePlaygroundRequest configurePlaygroundRequest)
    {
        try
        {
            if (configurePlaygroundRequest is null) return null;

            var createdPlayground = await ConfigurePlayground(configurePlaygroundRequest);

            await ConfigureShop(configurePlaygroundRequest);

            await ConfigureTeams(configurePlaygroundRequest);
            return createdPlayground;
        }
        catch (Exception)
        {

            return null;
        }
    }

    private async Task<PlaygroundEntity> ConfigurePlayground(ConfigurePlaygroundRequest configurePlaygroundRequest)
    {
        // Create Playground
        var rounds = new List<RoundEntity>();
        foreach (var round in configurePlaygroundRequest.Rounds)
        {
            rounds.Add(new RoundEntity
            {
                RoundNumber = round.Key,
                Duration = round.Value,
                Progress = RoundState.Pending.Name
            });
        }

        var playground = new CreatePlaygroundRequest
        {
            HostRoomCode = configurePlaygroundRequest.HostRoomCode,
            PlayerRoomCode = configurePlaygroundRequest.PlayerRoomCode,
            NumberOfTeam = configurePlaygroundRequest.NumberOfTeam,
            TeamToken = configurePlaygroundRequest.TeamToken,
            Rounds = rounds
        };

        var createdPlayground = await _playgroundService.Create(playground);
        return createdPlayground;
    }

    private async Task ConfigureTeams(ConfigurePlaygroundRequest configurePlaygroundRequest)
    {
        for (int i = 1; i < configurePlaygroundRequest.NumberOfTeam + 1; i++)
        {
            // Create Team
            var team = new CreateTeamRequest
            {
                TeamNumber = i,
                Tokens = configurePlaygroundRequest.TeamToken,
                HostRoomCode = configurePlaygroundRequest.HostRoomCode,
                PlayerRoomCode = configurePlaygroundRequest.PlayerRoomCode
            };

            var createdTeam = await _teamService.Create(team);

            // Create Team Stocks
            var teamProducts = configurePlaygroundRequest.Shop.Select(p => new ProductEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                ProductName = p.ProductName,
                Price = p.Price,
                RemainingStock = 0,
                OwnerId = createdTeam.Id,
                CreationDate = DateTime.Now,
                ModifiedDate = null
            }).ToList();

            var teamProductResults = await _productService.CreateAllThreeProducts(teamProducts);
        }
    }

    private async Task ConfigureShop(ConfigurePlaygroundRequest configurePlaygroundRequest)
    {
        // Create Shop
        var shop = new CreateShopRequest
        {
            HostRoomCode = configurePlaygroundRequest.HostRoomCode,
            PlayerRoomCode = configurePlaygroundRequest.PlayerRoomCode,
            Tokens = configurePlaygroundRequest.ShopToken
        };

        var createdShop = await _shopService.CreateAsync(shop);

        // Create Shop Stocks
        var shopProducts = configurePlaygroundRequest.Shop.Select(p => new ProductEntity
        {
            Id = ObjectId.GenerateNewId().ToString(),
            ProductName = p.ProductName,
            Price = p.Price,
            RemainingStock = p.RemainingStock,
            OwnerId = createdShop.Id,
            CreationDate = DateTime.Now,
            ModifiedDate = null
        }).ToList();

        var shopProductResults = await _productService.CreateAllThreeProducts(shopProducts);
    }
}