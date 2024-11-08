using SnowFlake.DAO;
using SnowFlake.Dtos;
using SnowFlake.Repositories.Common;

namespace SnowFlake.Repository;

public class TeamRepository : BaseRepository<TeamEntity>, ITeamRepository
{
    private readonly SnowFlakeDbContext _snowFlakeDbContext;
    private IBaseRepository<TeamEntity> _baseRepositoryImplementation;

    public TeamRepository(SnowFlakeDbContext snowFlakeDbContext):base(snowFlakeDbContext)
    {
        _snowFlakeDbContext = snowFlakeDbContext;
    }
}