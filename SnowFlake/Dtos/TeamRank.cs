namespace SnowFlake.Dtos;

public class TeamRank : BaseEntity
{
    public int TeamNumber { get; set; }
    public int RemainingTokens { get; set; }
    public int Rank { get; set; }
    public int TotalSnowFlakeSales { get; set; }
}