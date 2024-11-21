using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Player.GetPlayer;

public class GetPlayerResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? FirebaseId { get; set; }
    public string TeamId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }

}