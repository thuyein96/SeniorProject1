namespace SnowFlake.Dtos.APIs.Team.GetTeams;

public class TeamWithProducts
{
    public string Id { get; set; }
    public int TeamNumber { get; set; }
    public string? HostRoomCode { get; set; }
    public string? PlayerRoomCode { get; set; }
    public int? Tokens { get; set; }
    public List<string>? Members { get; set; }
    public List<Dtos.Product>? TeamStocks { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? ModifiedTime { get; set; }
}