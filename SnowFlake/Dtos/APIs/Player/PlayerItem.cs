using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Player;

public class PlayerItem
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Email { get; set; }
    public string? TeamId { get; set; }
    public string RoomCode { get; set; }
    public string PlaygroundId { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
