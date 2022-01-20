using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;
using Shared;

namespace Application.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task CreateAsync(T entity);
        Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate);
        Task<T> SingleByIdAsync(int id);
        void UpdateRange(IEnumerable<T> entites);
        void RemoveRange(IEnumerable<T> entities);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
    }
}