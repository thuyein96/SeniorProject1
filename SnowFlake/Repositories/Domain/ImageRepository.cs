using SnowFlake.DAO;
using SnowFlake.Dtos;
using SnowFlake.Repositories.Common;

namespace SnowFlake.Repositories.Domain;

public class ImageRepository : BaseRepository<ImageEntity>, IImageRepository
{
    private readonly SnowFlakeDbContext _snowFlakeDbContext;

    public ImageRepository(SnowFlakeDbContext snowFlakeDbContext) : base(snowFlakeDbContext)
    {
        _snowFlakeDbContext = snowFlakeDbContext;
    }
}
