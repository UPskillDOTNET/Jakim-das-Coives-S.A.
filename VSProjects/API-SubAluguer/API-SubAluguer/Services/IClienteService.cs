using API_SubAluguer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Services
{
    public interface IClienteService
    {
        public Task<ActionResult<IEnumerable<Cliente>>> GetAllAsync();
        public Task<Cliente> GetByIdAsync(int id);
        public Task PutAsync(Cliente cliente);
        public Task<Cliente> PostAsync(Cliente cliente);
        public Task DeleteAsync(int id);
    }
}
