using SnowFlake.DAO;
using SnowFlake.Dtos;
using SnowFlake.Repositories.Common;

namespace SnowFlake.Repositories.Domain
{
    public class PlaygroundRepository : BaseRepository<PlaygroundEntity>, IPlaygroundRepository
    {
        private readonly SnowFlakeDbContext _snowFlakeDbContext;

        public PlaygroundRepository(SnowFlakeDbContext snowFlakeDbContext) : base(snowFlakeDbContext)
        {
            _snowFlakeDbContext = snowFlakeDbContext;
        }
    }
}
