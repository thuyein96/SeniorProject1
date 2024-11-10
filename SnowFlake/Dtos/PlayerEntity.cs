using System.ComponentModel.DataAnnotations.Schema;

namespace SnowFlake.Dtos;
[Table("Players")]
public class PlayerEntity : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string StudentId { get; set; }
    public string? Major { get; set; }
    public string? Faculty { get; set; }
    public string? FirebaseId { get; set; }
    public string TeamId { get; set; }
    public string? ProfileImageUrl { get; set; }
}