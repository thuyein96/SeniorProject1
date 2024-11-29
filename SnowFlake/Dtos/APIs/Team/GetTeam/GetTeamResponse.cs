using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SnowFlake.Dtos.APIs.Team.GetTeam;

public class GetTeamResponse
{
    public bool Success { get; set; }
    public TeamEntity Message { get; set; }
}