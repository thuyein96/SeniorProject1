using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Leaderboard.CreateLeaderboard;

namespace SnowFlake.Services;

public interface ILeaderboardService
{
    Task<LeaderboardEntity> GetLeaderboardByHostRoomCode(string hostRoomCode);
    Task<LeaderboardEntity> GetLeaderboardByPlayerRoomCode(string playerRoomCode);
    Task<LeaderboardEntity> CreateLeaderboard(CreateLeaderboardRequest createLeaderboardRequest);
}