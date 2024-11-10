using SnowFlake.DAO;
using SnowFlake.Dtos;
using SnowFlake.Repositories.Common;

namespace SnowFlake.Repository;

public class PlayerRepository : BaseRepository<PlayerEntity>, IPlayerRepository
{
    private readonly SnowFlakeDbContext _snowFlakeDbContext;

    public PlayerRepository(SnowFlakeDbContext snowFlakeDbContext):base(snowFlakeDbContext)
    {
        _snowFlakeDbContext = snowFlakeDbContext;
    }
}