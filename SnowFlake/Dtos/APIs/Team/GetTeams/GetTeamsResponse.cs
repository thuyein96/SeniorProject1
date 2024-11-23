using SnowFlake.Dtos.APIs.Team.GetTeam;

namespace SnowFlake.Dtos.APIs.Team.GetTeams;

public class GetTeamsResponse
{
    public List<GetTeamResponse> Teams { get; set; }
}