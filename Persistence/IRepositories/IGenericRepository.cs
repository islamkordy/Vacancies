using System.Linq.Expressions;

namespace Persistence.IRepositories;

public interface IGenericRepository<T> where T : class
{
    Task Add(T entity);
    Task AddRange(IEnumerable<T> entities);
    //(IQueryable<T> data, int count) Get(BaseSpecifications<T> specifications);
    Task<T?> GetById(int id);
    Task<T?> GetByGuid(Guid id);
    Task<T?> GetObj(Expression<Func<T, bool>> filter);
    Task<List<T?>> GetObjs(Expression<Func<T, bool>> filter);
    Task<bool> IsExist(Expression<Func<T, bool>> filter);
    void Delete(T entity);
    void Update(T entity);
    Task<bool> Save();
    void UpdateRange(IEnumerable<T> entities);
    void DeleteRange(IEnumerable<T> entities);
}
