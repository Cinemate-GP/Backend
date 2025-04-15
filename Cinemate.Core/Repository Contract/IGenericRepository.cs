using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Repository_Contract
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        IQueryable<TEntity> GetQueryable();

        Task<TEntity> GetAsync(int keyValues);


        Task AddAsync(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        void Delete(TEntity entity);

        Task<int> CountEntity();

        void DeleteAll();


    }
}
