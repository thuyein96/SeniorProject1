using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SnowFlake.DAO;

namespace SnowFlake.Repositories.Common;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{   
    private readonly SnowFlakeDbContext _snowFlakeDbContext;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(SnowFlakeDbContext snowFlakeDbContext)
    {
        _snowFlakeDbContext = snowFlakeDbContext;
        _dbSet = _snowFlakeDbContext.Set<T>();
    }

    public async Task Create(T entity)
    {
        _snowFlakeDbContext.Add<T>(entity);
    }

    public async Task Delete(T entity)
    {
        _snowFlakeDbContext.Remove<T>(entity);
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return _dbSet.AsNoTracking().AsEnumerable();
    }

    public async Task<IEnumerable<T>> GetBy(Expression<Func<T, bool>> expression)
    {
        return _dbSet.AsNoTracking().Where(expression).AsEnumerable();
    }

    public async Task Update(T entity)
    {
        _snowFlakeDbContext.Update<T>(entity);
    }
}