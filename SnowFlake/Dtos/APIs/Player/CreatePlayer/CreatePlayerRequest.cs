using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs;

public class CreatePlayerRequest
{
    public string Name { get; set; }
    public string? TeamId { get; set; }
    public string? RoomdCode { get; set; }
}