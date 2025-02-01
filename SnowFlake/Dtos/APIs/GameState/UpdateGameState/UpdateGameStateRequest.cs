namespace SnowFlake.Dtos.APIs.GameState.UpdateGameState;

public class UpdateGameStateRequest
{
    public string HostRoomCode { get; set; }
    public string CurrentGameState { get; set; }
    public int CurrentRoundNumber { get; set; }
}
