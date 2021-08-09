using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace DataModel.Contracts
{
    internal interface IUnitOfWork<out TContext>
        where TContext : DbContext, new()
    {
        TContext Context { get; }
        void CreateTransaction();
        void Commit();
        void Rollback();
        Task Save();
    }
}
