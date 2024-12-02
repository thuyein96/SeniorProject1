namespace SnowFlake.Dtos.APIs.Team.GetTeams;

public class GetTeamsResponse
{
    public bool Success { get; set; }
    public List<TeamEntity> Message { get; set; }
}