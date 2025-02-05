namespace SnowFlake.Dtos.APIs.Leaderboard.GetLeaderboard;

public class GetLeaderboardResponse : BaseResponse
{
    public List<TeamRankDetails> Message { get; set; }
}