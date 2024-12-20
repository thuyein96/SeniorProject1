using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Team.GetTeam;

public class GetTeamRequest
{
    public string TeamId { get; set; }
}