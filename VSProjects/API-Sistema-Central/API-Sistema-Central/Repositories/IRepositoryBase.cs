using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Repositories
{
    public interface IRepositoryBase<T>
    {
        public Task<ActionResult<IEnumerable<T>>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        public Task PutAsync(T entity);
        public Task<T> PostAsync(T entity);
        public Task DeleteAsync(int id);
    }
}
