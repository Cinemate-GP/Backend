using Cinemate.Core.Repository_Contract;
using Cinemate.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Repository.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public ApplicationDbContext _context { get; }

        public GenericRepository(ApplicationDbContext storeDbContext)
        {
            _context = storeDbContext;
        }
        public IQueryable<TEntity> GetQueryable()
        {
            return _context.Set<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {

            return await _context.Set<TEntity>().ToListAsync();
        }
        public async Task<TEntity> GetAsync(int keyValues)
        {
            return await _context.Set<TEntity>().FindAsync(keyValues);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            if (entity == null) return null;

            _context.Set<TEntity>().Update(entity);
            var changes = await _context.SaveChangesAsync(); // Save changes to persist the update

            return entity; // Return true if changes were made, otherwise false
        }


        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void DeleteAll()
        {
            var dbSet = _context.Set<TEntity>();
            _context.RemoveRange(dbSet); // Deletes all entities in the DbSet

        }

        public async Task<int> CountEntity()
        {

            return await _context.Set<TEntity>().CountAsync();
        }


    }
}
