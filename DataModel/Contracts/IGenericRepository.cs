using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataModel.Contracts
{
    internal interface IGenericRepository<T> where T : class
    {
        Task<IList<T>> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = ""
            );
        Task<T> Get(Expression<Func<T, bool>> expression, string includeProperties = "");
        Task<bool> IsExists(Expression<Func<T, bool>> expression = null);
        void Insert(T obj);
        void Update(T obj);
        void Delete(T obj);
    }
}
