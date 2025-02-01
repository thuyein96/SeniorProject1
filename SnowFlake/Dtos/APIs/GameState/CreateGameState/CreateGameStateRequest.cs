namespace SnowFlake.Dtos.APIs.GameState.CreateGameState;

public class CreateGameStateRequest
{
    public string HostRoomCode { get; set; }
    public string PlayerRoomCode { get; set; }
    public string CurrentGameState { get; set; }
    public int CurrentRoundNumber { get; set; }
}
