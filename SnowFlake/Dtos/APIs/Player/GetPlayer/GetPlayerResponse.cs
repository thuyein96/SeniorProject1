namespace SnowFlake.Dtos.APIs.Player.GetPlayer;

public class GetPlayerResponse
{
    public Guid PlayerId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string StudentId { get; set; }
    public string? Major { get; set; }
    public string? Faculty { get; set; }
    public Guid TeamId { get; set; }
    public string ProfileImageUrl { get; set; }
}