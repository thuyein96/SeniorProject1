using SnowFlake.Dtos.APIs.Team.GetTeams;

namespace SnowFlake.Dtos.APIs.Team.GetTeam;

public class GetTeamResponse : BaseResponse
{
    public TeamWithProducts Message { get; set; }
}