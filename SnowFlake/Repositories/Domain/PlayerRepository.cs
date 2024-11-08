using SnowFlake.DAO;

namespace SnowFlake.Repository;

public class PlayerRepository : IPlayerRepository
{
    private readonly SnowFlakeDbContext _snowFlakeDbContext;

    public PlayerRepository(SnowFlakeDbContext snowFlakeDbContext)
    {
        _snowFlakeDbContext = snowFlakeDbContext;
    }
}