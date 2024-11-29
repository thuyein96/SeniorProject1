using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Playground;

public class CreatePlaygroundRequest
{
    public ObjectId Id { get; set; }
    public string RoomCode { get; set; }
    public Dictionary<string, string> Rounds { get; set; }
    public int MaxTeam { get; set; }
    public int TeamToken { get; set; }
}