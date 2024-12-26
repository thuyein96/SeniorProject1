using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;

[Collection("GameState")]
public class GameStateEntity : BaseEntity
{
    public string HostRoomCode { get; set; }
    public string PlayerRoomCode { get; set; }
    public string CurrentGameState { get; set; }

}
