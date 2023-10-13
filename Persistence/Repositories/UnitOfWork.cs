using Persistence.Context;
using Persistence.IRepositories;
using System.Collections;

namespace Persistence.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ContextWrite _contextWrite;
    private readonly ContextRead _contextRead;

    private Hashtable _repositories;
    public UnitOfWork(ContextWrite contextWrite, ContextRead contextRead)
    {
        _contextWrite = contextWrite;
        _contextRead = contextRead;
    }

    public async Task<int> Complete() {
        return
        (await _contextWrite.SaveChangesAsync())+(await _contextRead.SaveChangesAsync());
        
    } 

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (_repositories is null)
            _repositories = new Hashtable();

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repository = new GenericRepository<TEntity>(_contextRead,_contextWrite);
            _repositories.Add(type, repository);
        }

        return (IGenericRepository<TEntity>)_repositories[type];
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
