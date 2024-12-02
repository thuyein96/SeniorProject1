using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Playground;

public class CreatePlaygroundRequest
{
    public string HostRoomCode { get; set; }
    public string PlayerRoomCode { get; set; }
    public string HostId { get; set; }
    public Dictionary<string, string> Rounds { get; set; }
    public int NumberOfTeam { get; set; }
    public int MaxTeamMember { get; set; }
    public int TeamToken { get; set; }
}