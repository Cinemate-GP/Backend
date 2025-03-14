using Cinemate.Core.Repository_Contract;
using Cinemate.Repository.Data.Contexts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private Hashtable _repository;

        public UnitOfWork(ApplicationDbContext storeDbContext)
        {
            _context = storeDbContext;
            _repository = new Hashtable();
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);

            if (!_repository.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_context);
                _repository.Add(type, repository); // Use `type` as key
            }

            return _repository[type] as IGenericRepository<TEntity>;
        }
    }
}
