using SnowFlake.DAO;
using SnowFlake.Dtos;
using SnowFlake.Repositories.Common;

namespace SnowFlake.Repositories.Domain;

public class CartRepository : BaseRepository<CartEntity>, ICartRepository
{
    private readonly SnowFlakeDbContext _snowFlakeDbContext;

    public CartRepository(SnowFlakeDbContext snowFlakeDbContext) : base(snowFlakeDbContext)
    {
        _snowFlakeDbContext = snowFlakeDbContext;
    }
}