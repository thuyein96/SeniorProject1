using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Leaderboard;
using SnowFlake.UnitOfWork;

namespace SnowFlake.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly IUnitOfWork _unitOfWork;

    public LeaderboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LeaderboardEntity> Create(CreateLeaderboardRequest createLeaderboardRequest)
    {
        try
        {
            if (createLeaderboardRequest is null) return null;
            var leaderboard = new LeaderboardEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                TeamNumber = createLeaderboardRequest.TeamNumber,
                TeamRank = createLeaderboardRequest.TeamRank,
                RemainingTokens = createLeaderboardRequest.RemainingTokens,
                TotalSales = createLeaderboardRequest.TotalSales,
                CreationDate = DateTime.Now,
                ModifiedDate = null
            };
            await _unitOfWork.LeaderboardRepository.Create(leaderboard);
            await _unitOfWork.Commit();
            return leaderboard;
        }
        catch (Exception)
        {
            return null;
        }
    }
}