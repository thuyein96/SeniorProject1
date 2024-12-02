using SnowFlake.DAO;
using SnowFlake.Dtos;
using SnowFlake.Repositories.Common;

namespace SnowFlake.Repositories.Domain;

public class RoundRepository : BaseRepository<RoundEntity>, IRoundRepository
{
    private readonly SnowFlakeDbContext _snowFlakeDbContext;
    private IBaseRepository<RoundEntity> _baseRepositoryImplementation;

    public RoundRepository(SnowFlakeDbContext snowFlakeDbContext) : base(snowFlakeDbContext)
    {
        _snowFlakeDbContext = snowFlakeDbContext;
    }
}
