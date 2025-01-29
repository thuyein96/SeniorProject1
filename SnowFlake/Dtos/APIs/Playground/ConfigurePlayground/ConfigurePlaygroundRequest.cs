using SnowFlake.Dtos;

namespace SnowFlake.Dtos.APIs.Playground.ConfigurePlayground;

public class ConfigurePlaygroundRequest
{
    public string HostRoomCode { get; set; }
    public string PlayerRoomCode { get; set; }
    public Dictionary<int, string> Rounds { get; set; }
    public int NumberOfTeam { get; set; }
    public int TeamToken { get; set; }
    public int ShopToken { get; set; }
    public List<Dtos.Product> Shop { get; set; }
}