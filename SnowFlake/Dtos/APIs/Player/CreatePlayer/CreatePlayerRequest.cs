namespace SnowFlake.Dtos.APIs;

public class CreatePlayerRequest
{
    public Guid PlayerId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string StudentId { get; set; }
    public string? Major { get; set; }
    public string? Faculty { get; set; }
    public Guid TeamId { get; set; }
    public string ProfileImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedOn { get; set; }
}