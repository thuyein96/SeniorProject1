using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Team.CreateTeam;

public class CreateTeamResponse
{
    public bool Success { get; set; }
    public TeamEntity? Message { get; set; }
}