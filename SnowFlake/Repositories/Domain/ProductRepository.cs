using SnowFlake.DAO;
using SnowFlake.Dtos;
using SnowFlake.Repositories.Common;

namespace SnowFlake.Repositories.Domain;

public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
{
    private readonly SnowFlakeDbContext _snowFlakeDbContext;
    public ProductRepository(SnowFlakeDbContext snowFlakeDbContext) : base(snowFlakeDbContext)
    {
        _snowFlakeDbContext = snowFlakeDbContext;
    }
}