using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Team.DeleteTeam;

public class DeleteTeamRequest
{
    public ObjectId TeamId { get; set; }
}