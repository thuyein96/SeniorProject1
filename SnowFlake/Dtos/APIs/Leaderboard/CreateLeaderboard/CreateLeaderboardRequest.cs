namespace SnowFlake.Dtos.APIs.Leaderboard.CreateLeaderboard;

public class CreateLeaderboardRequest
{
    public string HostRoomCode { get; set; }
    public string PlayerRoomCode { get; set; }
    public List<TeamRank> TeamRanks { get; set; }
}