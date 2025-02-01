using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Product.GetProducts;
using SnowFlake.Dtos.APIs.Team.GetTeams;
using SnowFlake.Dtos.APIs.Team.GetTeamsByRoomCode;
using SnowFlake.Services;

namespace SnowFlake.Managers;

public class TeamManager : ITeamManager
{
    private readonly ITeamService _teamService;
    private readonly IProductService _productService;

    public TeamManager(ITeamService teamService,
                       IProductService productService)
    {
        _teamService = teamService;
        _productService = productService;
    }

    public async Task<List<ProductEntity>> GetProductsByTeam(GetProductsByTeamRequest getProductsByTeamRequest)
    {
        var team = await _teamService.GetTeam(getProductsByTeamRequest.TeamNumber, getProductsByTeamRequest.PlayerRoomCode, null);

        if (team is null) return null;

        return await _productService.GetProductsByOwnerId(team.Id);
    }

    public async Task<List<TeamWithProducts>> GetTeamWithProducts(GetTeamsByRoomCodeRequest getTeamsByRoomCodeRequest)
    {
        var teams = await _teamService.GetTeamsByRoomCode(getTeamsByRoomCodeRequest);

        if (teams is null || teams.Count == 0) return null;
        var teamsWithProducts = new List<TeamWithProducts>();
        foreach (var team in teams)
        {
            var products = (await _productService.GetProductsByOwnerId(team.Id)).Select(p => new Product
            {
                ProductName = p.ProductName,
                Price = p.Price,
                RemainingStock = p.RemainingStock,
                
            }).ToList();
            teamsWithProducts.Add(new TeamWithProducts
            {
                Id = team.Id,
                TeamNumber = team.TeamNumber,
                HostRoomCode = team.HostRoomCode,
                PlayerRoomCode = team.PlayerRoomCode,
                Tokens = team.Tokens,
                Members = team.Members,
                TeamStocks = products,
                CreationTime = team.CreationDate,
                ModifiedTime = team.ModifiedDate
            });
        }

        return teamsWithProducts;
    }
}