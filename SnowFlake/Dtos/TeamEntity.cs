using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;

[Collection("Teams")]
public class TeamEntity : BaseEntity
{
    public string PlaygroundId { get; set; }
    public int TeamNumber { get; set; }
    public int MaxMembers { get; set; }
    public int Tokens { get; set; }
}