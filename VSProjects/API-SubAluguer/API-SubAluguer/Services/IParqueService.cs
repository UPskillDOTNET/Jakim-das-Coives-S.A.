using API_SubAluguer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Services
{
    public interface IParqueService
    {
        public Task<ActionResult<IEnumerable<Parque>>> GetAllAsync();
        public Task<Parque> GetByIdAsync(int id);
        public Task PutAsync(Parque parque);
        public Task<Parque> PostAsync(Parque parque);
        public Task DeleteAsync(int id);
    }
}