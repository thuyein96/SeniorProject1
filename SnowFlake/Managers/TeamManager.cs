using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Team.SearchPlayerInTeam;
using SnowFlake.Dtos.APIs.Product.GetProducts;
using SnowFlake.Dtos.APIs.Team.GetTeam;
using SnowFlake.Dtos.APIs.Team.GetTeams;
using SnowFlake.Dtos.APIs.Team.GetTeamsByRoomCode;
using SnowFlake.Services;
using SnowFlake.Utilities;
using PlayerItem = SnowFlake.Dtos.APIs.Player.PlayerItem;

namespace SnowFlake.Managers;

public class TeamManager : ITeamManager
{
    private readonly ITeamService _teamService;
    private readonly IProductService _productService;
    private readonly IPlayerService _playerService;
    private readonly IImageService _imageService;

    public TeamManager(ITeamService teamService,
                       IProductService productService,
                       IPlayerService playerService,
                       IImageService imageService)
    {
        _teamService = teamService;
        _productService = productService;
        _playerService = playerService;
        _imageService = imageService;
    }

    public async Task<List<ProductEntity>> GetProductsByTeam(GetProductsByTeamRequest getProductsByTeamRequest)
    {
        var team = await _teamService.GetTeam(getProductsByTeamRequest.TeamNumber, getProductsByTeamRequest.PlayerRoomCode, null);

        if (team is null) return null;

        return await _productService.GetProductsByOwnerId(team.Id);
    }

    public async Task<List<TeamWithProducts>> GetTeamsWithProducts(GetTeamsByRoomCodeRequest getTeamsByRoomCodeRequest)
    {
        var teams = await _teamService.GetTeamsByRoomCode(getTeamsByRoomCodeRequest);

        if (teams is null || teams.Count == 0) return null;
        var teamsWithProducts = new List<TeamWithProducts>();
        foreach (var team in teams)
        {
            teamsWithProducts.Add(await GetTeamDetails(team));
        }

        return teamsWithProducts;
    }

    public async Task<GetTeamResponse> GetTeamWithProducts(GetTeamRequest getTeamRequest)
    {
        var team = await _teamService.GetTeam(getTeamRequest.TeamNumber, getTeamRequest.PlayerRoomCode, getTeamRequest.HostRoomCode);
        if (team is null) return null;

        var teamsDetails = await GetTeamDetails(team);

        return teamsDetails is not null 
            ? new GetTeamResponse
            {
                Success = true,
                Message = teamsDetails
            }
            : new GetTeamResponse
            {
                Success = false,
                Message = null
            };
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

    private async Task<TeamWithProducts> GetTeamDetails(TeamEntity team)
    {
        var products = (await _productService.GetProductsByOwnerId(team.Id)).Select(p => new Product
        {
            ProductName = p.ProductName,
            Price = p.Price,
            RemainingStock = p.RemainingStock,

        }).ToList();

        var members = (await _playerService.GetPlayersByTeamId(team.Id)).Select(p => p.PlayerName).ToList();
        var images = (await _imageService.GetImagesByTeamId(team.Id, ImageBuyingStatus.Pending.Name));

        return new TeamWithProducts
        {
            Id = team.Id,
            TeamNumber = team.TeamNumber,
            HostRoomCode = team.HostRoomCode,
            PlayerRoomCode = team.PlayerRoomCode,
            Tokens = team.Tokens,
            Members = members,
            Images = images.Select(i => i.SnowFlakeImageUrl).ToList(),
            TeamStocks = products,
            CreationTime = team.CreationDate,
            ModifiedTime = team.ModifiedDate
        };
    }
}