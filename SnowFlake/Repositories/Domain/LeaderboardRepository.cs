using SnowFlake.DAO;
using SnowFlake.Dtos;
using SnowFlake.Repositories.Common;

namespace SnowFlake.Repositories.Domain
{
    public class LeaderboardRepository : BaseRepository<LeaderboardEntity>, ILeaderboardRepository
    {
        private readonly SnowFlakeDbContext _snowFlakeDbContext;

        public LeaderboardRepository(SnowFlakeDbContext snowFlakeDbContext) : base(snowFlakeDbContext)
        {
            _snowFlakeDbContext = snowFlakeDbContext;
        }
    }
}
