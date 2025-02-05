using SnowFlake.DAO;
using SnowFlake.Dtos;
using SnowFlake.Repositories.Common;

namespace SnowFlake.Repositories.Domain;

public class TransactionRepository : BaseRepository<TransactionEntity>, ITransactionRepository
{
    private readonly SnowFlakeDbContext _snowFlakeDbContext;

    public TransactionRepository(SnowFlakeDbContext snowFlakeDbContext) : base(snowFlakeDbContext)
    {
        _snowFlakeDbContext = snowFlakeDbContext;
    }
}