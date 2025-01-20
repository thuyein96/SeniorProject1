using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Player;

public class PlayerItem
{
    public string Id { get; set; }
    public string PlayerName { get; set; }
    public string? TeamId { get; set; }
    public int? TeamNumber { get; set; }
    public string PlayerRoomCode { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
