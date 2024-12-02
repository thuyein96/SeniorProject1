using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Playground;

public class CreatePlaygroundRequest
{
    public ObjectId Id { get; set; }
    public string HostRoomCode { get; set; }
    public string PlayerRoomCode { get; set; }
    public string HostId { get; set; }
    public Dictionary<string, string> Rounds { get; set; }
    public int MaxTeam { get; set; }
    public int TeamToken { get; set; }
}