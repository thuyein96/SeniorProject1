namespace SnowFlake.Dtos.APIs;

public class CreatePlayerResponse
{
    public Guid PlayerId { get; set; }
    public string PlayerName { get; set; }
    public string Email { get; set; }
    public Guid TeamId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}