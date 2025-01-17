namespace SnowFlake.Dtos.APIs.Player.AddPlayerToTeam;

public class ManagePlayerRequest
{
    public string PlayerName { get; set; }
    public int TeamNumber { get; set; }
    public string PlayerRoomCode { get; set; }
    public string Status { get; set; }
}
