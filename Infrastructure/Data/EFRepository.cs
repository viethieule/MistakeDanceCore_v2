using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<TEntity> SingleByIdAsync(int id)
        {
            return await _context.Set<TEntity>().SingleAsync(x => x.Id == id);
        }

        public async Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public void UpdateRange(IEnumerable<TEntity> entites)
        {
            _context.Set<TEntity>().UpdateRange(entites);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }
    }
}
