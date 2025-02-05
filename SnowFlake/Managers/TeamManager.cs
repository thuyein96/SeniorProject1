using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Team.SearchPlayerInTeam;
using SnowFlake.Dtos.APIs.Product.GetProducts;
using SnowFlake.Dtos.APIs.Team.GetTeams;
using SnowFlake.Dtos.APIs.Team.GetTeamsByRoomCode;
using SnowFlake.Services;
using PlayerItem = SnowFlake.Dtos.APIs.Player.PlayerItem;

namespace SnowFlake.Managers;

public class TeamManager : ITeamManager
{
    private readonly ITeamService _teamService;
    private readonly IProductService _productService;
    private readonly IPlayerService _playerService;

    public TeamManager(ITeamService teamService,
                       IProductService productService,
                       IPlayerService playerService)
    {
        _teamService = teamService;
        _productService = productService;
        _playerService = playerService;
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

            var members = (await _playerService.GetPlayersByTeamId(team.Id)).Select(p => p.PlayerName).ToList();

            teamsWithProducts.Add(new TeamWithProducts
            {
                Id = team.Id,
                TeamNumber = team.TeamNumber,
                HostRoomCode = team.HostRoomCode,
                PlayerRoomCode = team.PlayerRoomCode,
                Tokens = team.Tokens,
                Members = members,
                TeamStocks = products,
                CreationTime = team.CreationDate,
                ModifiedTime = team.ModifiedDate
            });
        }

        return teamsWithProducts;
    }

    public async Task<string> IsTeamHasPlayer(SearchPlayerRequest searchPlayerRequest)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchPlayerRequest.PlayerRoomCode)) return string.Empty;

            var team = (await _teamService.GetTeamsByRoomCode(new GetTeamsByRoomCodeRequest
            {
                PlayerRoomCode = searchPlayerRequest.PlayerRoomCode
            })).FirstOrDefault();
            if (team is null) return string.Empty;

            var hasPlayer = (await _playerService.GetPlayersByTeamId(team.Id)).Any(p => p.PlayerName.Equals(searchPlayerRequest.PlayerName));

            return hasPlayer ? "Player already exists in the team" : string.Empty;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
}