using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Team.GetTeam;

public class GetTeamRequest
{
    public ObjectId TeamId { get; set; }
}