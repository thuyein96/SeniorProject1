using SnowFlake.Dtos.APIs.Player.GetPlayer;

namespace SnowFlake.Dtos.APIs.Player.GetPlayerList;

public class GetPlayersResponse
{
    public bool Success { get; set; }
    public List<PlayerItem> Message { get; set; }
}