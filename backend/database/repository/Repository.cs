using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace backend.database.repository;
public abstract class Repository<T>(DbContext  context) : IRepository<T> where T : class
{
    public async Task<bool> AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        return context.SaveChangesAsync().IsCompleted;
    }
    public async Task<bool> RemoveAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        return context.SaveChangesAsync().IsCompleted;        
    }

    public IQueryable<T> Get(Expression<Func<T,bool>>? filter, bool tracked = false)
    {
        IQueryable<T> query = context.Set<T>();
        if(filter != null)
        {
            query = tracked ? query.Where(filter) : query.AsNoTracking().Where(filter);
        }
        return query;
    }
    
    public async Task<int> CountAsync() => context.Set<T>().Count();
}