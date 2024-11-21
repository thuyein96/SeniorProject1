using SnowFlake.Dtos.APIs.Player.GetPlayer;

namespace SnowFlake.Dtos.APIs.Player.GetPlayerList;

public class GetPlayersResponse
{
    public List<GetPlayerResponse> Players { get; set; }
}