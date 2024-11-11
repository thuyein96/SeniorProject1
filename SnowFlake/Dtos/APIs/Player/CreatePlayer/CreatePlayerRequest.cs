namespace SnowFlake.Dtos.APIs;

public class CreatePlayerRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string StudentId { get; set; }
    public string? Major { get; set; }
    public string? Faculty { get; set; }
    public string TeamId { get; set; }
    public string FireBaseId { get; set; }
    public string? ProfileImageUrl { get; set; }
}