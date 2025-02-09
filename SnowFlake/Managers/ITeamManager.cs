using SnowFlake.Dtos.APIs.Product.GetProducts;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Team.GetTeam;
using SnowFlake.Dtos.APIs.Team.GetTeams;
using SnowFlake.Dtos.APIs.Team.GetTeamsByRoomCode;
using SnowFlake.Dtos.APIs.Team.SearchPlayerInTeam;

namespace SnowFlake.Managers;

public interface ITeamManager
{
    Task<List<ProductEntity>> GetProductsByTeam(GetProductsByTeamRequest getProductsByTeamRequest);
    Task<List<TeamWithProducts>> GetTeamsWithProducts(GetTeamsByRoomCodeRequest getTeamsByRoomCodeRequest);
    Task<TeamWithProducts> GetTeamWithProducts(GetTeamRequest getTeamRequest);
    Task<string> IsTeamHasPlayer(SearchPlayerRequest searchPlayerRequest);
}