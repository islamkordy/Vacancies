using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.IRepositories;
using System.Linq.Expressions;

namespace Persistence.Repositories;
internal class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ContextRead _contextRead;
    private readonly ContextWrite _contextWrite;
    internal DbSet<T> _entityRead;
    internal DbSet<T> _entityWrite;
    public GenericRepository(ContextRead contextRead, ContextWrite contextWrite)
    {
        _contextRead = contextRead;
        _contextWrite = contextWrite;
        _entityWrite = _contextWrite.Set<T>();
        _entityRead = _contextRead.Set<T>();
    }

    public async Task Add(T entity) => await _entityWrite.AddAsync(entity);
    public async Task AddRange(IEnumerable<T> entities) => await _entityWrite.AddRangeAsync(entities);
    public void Update(T entity) => _entityWrite.Update(entity);
    public void Delete(T entity) => _entityWrite.Remove(entity);
    //public (IQueryable<T> data, int count) Get(BaseSpecifications<T> specifications) => SpecificationEvaluator<T>.GetQuery(_entityRead, specifications);
    public async Task<T?> GetById(int id) => await _entityRead.FindAsync(id);
    public async Task<T?> GetByGuid(Guid id) => await _entityRead.FindAsync(id);
    public async Task<T?> GetObj(Expression<Func<T, bool>> filter) => await _entityRead.AsQueryable<T>().FirstOrDefaultAsync(filter);
    public async Task<List<T?>> GetObjs(Expression<Func<T, bool>> filter) => await _entityRead.Where(filter).AsQueryable<T>().ToListAsync();
    public async Task<bool> IsExist(Expression<Func<T, bool>> filter) => await _entityRead.AnyAsync(filter);
    public async Task<bool> Save() => await _contextWrite.SaveChangesAsync() > 0;
    public void UpdateRange(IEnumerable<T> entities) =>  _entityWrite.UpdateRange(entities);
    public void DeleteRange(IEnumerable<T> entities) => _entityWrite.RemoveRange(entities);
}
