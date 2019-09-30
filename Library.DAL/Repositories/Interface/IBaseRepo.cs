using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Library.DAL.Entities;

namespace Library.DAL.Repositories.Interface
{
    public interface IBaseRepo<T> where T : BaseEntity
    {
        Task<long> AddAsync(T entity);

        IQueryable<T> Find(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll();

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}
