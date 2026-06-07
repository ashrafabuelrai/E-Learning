using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Learning.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProp = null);
        Task<T> Get(Expression<Func<T, bool>> filter, string? includeProp = null, bool tracked = false);
        Task Add(T entity);
        Task Remove(T entity);
        Task RemoveRange(IEnumerable<T> entity);
    }
}
