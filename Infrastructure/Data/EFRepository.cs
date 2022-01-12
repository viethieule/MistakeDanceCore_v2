using Application.Interfaces;
using Shared;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EFRepository<TEntity> : IRepository<TEntity> where TEntity: BaseEntity
    {
        protected readonly DataContext _context;
        
        public EFRepository(DataContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }
    }
}
