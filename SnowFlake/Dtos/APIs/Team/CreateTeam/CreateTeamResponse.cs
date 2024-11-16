using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Team.CreateTeam;

public class CreateTeamResponse
{
    public ObjectId Id { get; set; }
    public string TeamNumber { get; set; }
    public int MaxMembers { get; set; }
    public int Tokens { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}