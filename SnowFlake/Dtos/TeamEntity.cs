using System.ComponentModel.DataAnnotations.Schema;

namespace SnowFlake.Dtos;

[Table("teams")]
public class TeamEntity : BaseEntity
{
    public string TeamNumber { get; set; }
    public int MaxMembers { get; set; }
    public int Tokens { get; set; }
    public string ProfileImageUrl { get; set; } 
}