using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Team.CreateTeam;

public class CreateTeamRequest
{
    public int TeamNumber { get; set; }
    public string HostRoomCode { get; set; }
    public string PlayerRoomCode { get; set; }
    public int Tokens { get; set; }
}