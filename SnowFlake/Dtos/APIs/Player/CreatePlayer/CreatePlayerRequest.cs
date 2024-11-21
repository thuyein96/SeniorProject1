using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs;

public class CreatePlayerRequest
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? FirebaseId { get; set; }
    public string TeamId { get; set; }

}