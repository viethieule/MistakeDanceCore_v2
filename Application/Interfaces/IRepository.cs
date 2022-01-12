using System.Threading.Tasks;
using Shared;

namespace Application.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task CreateAsync(T entity);
    }
}