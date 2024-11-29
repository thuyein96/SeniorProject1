using System.Linq.Expressions;

namespace SnowFlake.Repositories.Common;

public interface IBaseRepository<T> where T : class
{
    Task Create(T entity);
    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> GetBy(Expression<Func<T,bool>> expression);
    Task Update(T entity);
    Task Delete(T entity);
}