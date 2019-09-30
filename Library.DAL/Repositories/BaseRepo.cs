using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Library.DAL.Entities;
using Library.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL.Repositories
{
    public class BaseRepo<T> : IBaseRepo<T>
        where T : BaseEntity
    {
        private readonly DbContext _context;

        protected BaseRepo(DbContext context)
        {
            this._context = context;
        }

        public async Task<long> AddAsync(T entity)
        {
            var entry = this._context.Set<T>().Add(entity);
            entry.State = EntityState.Added;
            await this._context.SaveChangesAsync();
            return entity.Id;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return this._context.Set<T>().Where(x => !x.IsDeleted.GetValueOrDefault()).Where(predicate).AsQueryable().AsNoTracking();
        }

        public IQueryable<T> GetAll()
        {
            return this._context.Set<T>().AsNoTracking();
        }

        public async Task UpdateAsync(T entity)
        {
            this._context.Entry(entity).State = EntityState.Modified;
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            entity.IsDeleted = true;
            await this.UpdateAsync(entity);
        }
    }
}
