using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;
[Collection("Leaderboard")]
public class LeaderboardEntity : BaseEntity
{
    public int TeamNumber { get; set; }
    public int TeamRank { get; set; }
    public int RemainingTokens { get; set; }
    public int TotalSales { get; set; }
}
