namespace SnowFlake.Dtos.APIs.Leaderboard.CreateLeaderboard;

public class CreateLeaderboardResponse : BaseResponse
{
    public List<TeamRankDetails> Message { get; set; }
}