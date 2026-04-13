using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace backend.database.repository;
public abstract class Repository<T>(DbContext  context) : IRepository<T> where T : class
{
    public async Task<bool> AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        return context.SaveChangesAsync().IsCompletedSuccessfully;
    }
    public async Task<bool> RemoveAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        return context.SaveChangesAsync().IsCompletedSuccessfully;        
    }

    public IQueryable<T> Get(Expression<Func<T,bool>>? filter, int page, int amount, bool tracked = false)
    {
        IQueryable<T> query = context.Set<T>();
        if(filter != null)
        {
            query = tracked ? query.Where(filter) : query.AsNoTracking().Where(filter).Skip(page).Take(amount);
        }
        else
        {
            query = query.AsNoTracking().Skip(page).Take(amount);
        }

        return query;
    }    
    public async Task<int> CountAsync() => await context.Set<T>().CountAsync();

    public async Task<bool> AddInRangeAsync(IEnumerable<T> entity)
    {
        await context.Set<T>().AddRangeAsync(entity);
        return context.SaveChangesAsync().IsCompletedSuccessfully;
    }
}