using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SnowFlake.DAO;

namespace SnowFlake.Repositories.Common;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{   
    private readonly SnowFlakeDbContext _SnowFlakeDbContext;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(SnowFlakeDbContext snowFlakeDbContext)
    {
        _SnowFlakeDbContext = snowFlakeDbContext;
        _dbSet = _SnowFlakeDbContext.Set<T>();
    }

    public void Create(T entity)
    {
        _SnowFlakeDbContext.Add<T>(entity);
    }

    public void Delete(T entity)
    {
        _SnowFlakeDbContext.Remove<T>(entity);
    }

    public IEnumerable<T> GetAll()
    {
        return _dbSet.AsNoTracking().AsEnumerable();
    }

    public IEnumerable<T> GetBy(Expression<Func<T, bool>> expression)
    {
        return _dbSet.AsNoTracking().Where(expression).AsEnumerable();
    }

    public void Update(T entity)
    {
        _SnowFlakeDbContext.Update<T>(entity);
    }
}