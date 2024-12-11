using SnowFlake.DAO;
using SnowFlake.Dtos;
using SnowFlake.Repositories.Common;

namespace SnowFlake.Repositories.Domain
{
    public class GameStateRepository : BaseRepository<GameStateEntity>, IGameStateRepository
    {
        private readonly SnowFlakeDbContext _snowFlakeDbContext;
        public GameStateRepository(SnowFlakeDbContext snowFlakeDbContext) : base(snowFlakeDbContext)
        {
            _snowFlakeDbContext = snowFlakeDbContext;
        }
    }
}
