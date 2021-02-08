using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Services
{
    public interface ICartaoService
    {
        public Task<ActionResult<IEnumerable<Cartao>>> GetAllAsync();
        public Task<Cartao> GetByIdAsync(int id);
        public Task PutAsync(Cartao cartao);
        public Task<Cartao> PostAsync(Cartao cartao);
        public Task DeleteAsync(int id);
    }
}
