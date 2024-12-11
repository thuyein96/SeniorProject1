namespace SnowFlake.Dtos;

public class GameStateEntity : BaseEntity
{
    public string HostRoomCode { get; set; }
    public string PlayerRoomCode { get; set; }
    public string CurrentState { get; set; }

}
