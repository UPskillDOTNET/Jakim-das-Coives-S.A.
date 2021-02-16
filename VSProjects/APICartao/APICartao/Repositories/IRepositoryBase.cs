using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICartao.Repositories
{
    public interface IRepositoryBase<T>
    {
        Task<ActionResult<IEnumerable<T>>> FindAllAsync();
        Task<T> FindById(long id);

        Task<T> Create(T entity);
    }
}
