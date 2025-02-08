using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Team.DeleteTeam;

public class DeleteTeamRequest
{
    public string? HostRoomCode { get; set; }
    public string? PlayerRoomCode { get; set; }
    public int TeamNumber { get; set; }
}