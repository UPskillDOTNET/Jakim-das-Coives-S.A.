using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Repository
{
    public interface IRepositoryBase<T>
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        public Task PutAsync(T entity);
        public Task<T> PostAsync(T entity);
        public Task DeleteAsync(int id);
    }
}
