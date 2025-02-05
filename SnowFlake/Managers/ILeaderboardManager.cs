using SnowFlake.Dtos.APIs.Leaderboard;

namespace SnowFlake.Managers;

public interface ILeaderboardManager
{
    Task<List<TeamRankDetails>> CreateLeaderboard(string hostRoomCode);
    Task<List<TeamRankDetails>> GetLeaderboardByHostRoomCode(string hostRoomCode);
}