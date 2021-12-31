using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> CreateAsync(T entity);
    }
}