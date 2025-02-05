using SnowFlake.Dtos.APIs.Product;

namespace SnowFlake.Dtos.APIs.Leaderboard;

public class TeamRankDetails
{
    public int TeamNumber { get; set; }
    public int TeamRank { get; set; }
    public int RemainingTokens { get; set; }
    public int TotalSales { get; set; }
    public List<string> Players { get; set; }
    public List<ProductEntity> Stocks { get; set; }
    public List<string> Images { get; set; }
}