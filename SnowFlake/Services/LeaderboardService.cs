using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Leaderboard.CreateLeaderboard;
using SnowFlake.UnitOfWork;

namespace SnowFlake.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly IUnitOfWork _unitOfWork;

    public LeaderboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LeaderboardEntity> GetLeaderboardByHostRoomCode(string hostRoomCode)
    {
        if (string.IsNullOrWhiteSpace(hostRoomCode)) return null;
        var leaderboard = (await _unitOfWork.LeaderboardRepository.GetBy(l => l.HostRoomCode == hostRoomCode)).FirstOrDefault();
        return leaderboard;
    }

    public async Task<LeaderboardEntity> GetLeaderboardByPlayerRoomCode(string playerRoomCode)
    {
        if (string.IsNullOrWhiteSpace(playerRoomCode)) return null;
        var leaderboard = (await _unitOfWork.LeaderboardRepository.GetBy(l => l.PlayerRoomCode.Equals(playerRoomCode))).FirstOrDefault();
        return leaderboard;
    }

    public async Task<LeaderboardEntity> CreateLeaderboard(CreateLeaderboardRequest createLeaderboardRequest)
    {
        if (createLeaderboardRequest is null) return null;
        var leaderboard = new LeaderboardEntity
        {
            Id = ObjectId.GenerateNewId().ToString(),
            HostRoomCode = createLeaderboardRequest.HostRoomCode,
            PlayerRoomCode = createLeaderboardRequest.PlayerRoomCode,
            TeamRanks = createLeaderboardRequest.TeamRanks,
            CreationDate = DateTime.Now,
            ModifiedDate = null
        };

        await _unitOfWork.LeaderboardRepository.Create(leaderboard);
        await _unitOfWork.Commit();

        return leaderboard;
    }
}