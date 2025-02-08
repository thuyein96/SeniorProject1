using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Team.GetTeam;

public class GetTeamRequest
{
    public string? HostRoomCode { get; set; }
    public string? PlayerRoomCode { get; set; }
    public int TeamNumber { get; set; }
}