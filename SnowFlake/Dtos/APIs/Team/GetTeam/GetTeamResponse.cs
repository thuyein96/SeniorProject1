using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SnowFlake.Dtos.APIs.Team.GetTeam;

public class GetTeamResponse
{
    public string Id { get; set; }
    public string TeamNumber { get; set; }
    public int MaxMembers { get; set; }
    public int Tokens { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}