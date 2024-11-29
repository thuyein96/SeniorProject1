using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Player.GetPlayer;

public class GetPlayerResponse
{
    public bool Success { get; set; }
    public PlayerItem Message { get; set; }

}