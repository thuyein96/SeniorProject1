namespace SnowFlake.Dtos.APIs.GameState.UpdateGameState;

public class UpdateGameStateRequest
{
    public string Id { get; set; }
    public string? HostRoomCode { get; set; }
    public string? PlayerRoomCode { get; set; }
    public string CurrentState { get; set; }
}
