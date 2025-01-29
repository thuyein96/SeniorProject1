using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;

[Collection("Teams")]
public class TeamEntity : BaseEntity
{
    public int TeamNumber { get; set; }
    public string? HostRoomCode { get; set; }
    public string? PlayerRoomCode { get; set; }
    public int? Tokens { get; set; }
    public List<string>? Members { get; set; }
}