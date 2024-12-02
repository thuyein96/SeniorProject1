using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Player.UpdatePlayer;

public class UpdatePlayerRequest
{
    public string Id { get; set; }
    public string PlayerName { get; set; }
    public string Email { get; set; }
    public string TeamId { get; set; }
    public string RoomCode { get; set; }
    public string PlaygroundId { get; set; }
}