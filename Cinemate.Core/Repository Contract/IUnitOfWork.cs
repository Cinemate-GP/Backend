using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Repository_Contract
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();

        //Create Repository<T> Ans Returns

        IGenericRepository<TEntity> Repository<TEntity>() where TEntity: class;



    }
}
