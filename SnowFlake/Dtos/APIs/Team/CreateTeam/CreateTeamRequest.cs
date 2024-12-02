using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Team.CreateTeam;

public class CreateTeamRequest
{
    public int TeamNumber { get; set; }
    public int MaxMembers { get; set; }
    public int Tokens { get; set; }
    public string PlaygroundId { get; set; }
}