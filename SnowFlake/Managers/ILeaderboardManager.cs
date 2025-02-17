using SnowFlake.Dtos.APIs.Leaderboard;
using SnowFlake.Dtos.APIs.Leaderboard.CreateLeaderboard;
using SnowFlake.Dtos.APIs.Leaderboard.GetLeaderboard;
using SnowFlake.Dtos.APIs.Team.GetTeamsByRoomCode;

namespace SnowFlake.Managers;

public interface ILeaderboardManager
{
    Task<CreateLeaderboardResponse> CreateLeaderboard(string? hostRoomCode, string? playerRoomCode);
    Task<GetLeaderboardResponse> GetLeaderboard(string? hostRoomCode, string?  playerRoomCode);
}