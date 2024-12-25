using SnowFlake.DAO;
using SnowFlake.Dtos;
using SnowFlake.Repositories.Common;

namespace SnowFlake.Repositories.Domain;

public class ShopRepository : BaseRepository<ShopEntity>, IShopRepository
{
    private readonly SnowFlakeDbContext _snowFlakeDbContext;

    public ShopRepository(SnowFlakeDbContext snowFlakeDbContext) : base(snowFlakeDbContext)
    {
        _snowFlakeDbContext = snowFlakeDbContext;
    }
}
