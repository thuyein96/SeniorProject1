namespace SnowFlake.Dtos.APIs.Leaderboard;

public class CreateLeaderboardRequest
{
    public int TeamNumber { get; set; }
    public int TeamRank { get; set; }
    public int RemainingTokens { get; set; }
    public int TotalSales { get; set; }
}