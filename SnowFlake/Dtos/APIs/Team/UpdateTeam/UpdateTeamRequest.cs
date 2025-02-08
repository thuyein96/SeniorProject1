namespace SnowFlake.Dtos.APIs.Team.UpdateTeam;

public class UpdateTeamRequest
{
    public string? HostRoomCode { get; set; }
    public string? PlayerRoomCode { get; set; }
    public int TeamNumber { get; set; }
    public int? Tokens { get; set; }
}