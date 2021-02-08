using APICartao.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICartao.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected APICartaoContext _context { get; set; }

        public RepositoryBase(APICartaoContext theContext) => this._context = theContext;

        public async Task<ActionResult<IEnumerable<T>>> FindAllAsync() {
            return await this._context.Set<T>().ToListAsync();
        }
        public async Task<T> FindById(long id)
        {
            return await this._context.Set<T>().FindAsync(id);
        }

        public async Task<T> Create(T entity)
        {
            EntityEntry<T> entry;
            entry = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }
    }
}
