using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace backend.database.repository;
public interface IRepository<T>
{
    public Task<bool> AddAsync(T entity);
    public Task<bool> RemoveAsync(T entity);
    public IQueryable<T> Get(Expression<Func<T,bool>>? filter, bool tracked = false);
    public Task<int> CountAsync();
}