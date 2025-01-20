namespace SnowFlake.Dtos.APIs.Player.SearchPlayer;

public class SearchPlayerRequest
{
    public string PlayerName { get; set; }
    public int? TeamNumber { get; set; }
    public string? PlayerRoomCode { get; set; }
}
