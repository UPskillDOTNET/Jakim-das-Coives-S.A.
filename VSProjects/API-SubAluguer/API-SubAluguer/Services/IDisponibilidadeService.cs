using API_SubAluguer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Services
{
    public interface IDisponibilidadeService
    {
        public Task<ActionResult<IEnumerable<Disponibilidade>>> GetAllAsync();
        public Task<Disponibilidade> GetByIdAsync(int id);
        public Task PutAsync(Disponibilidade disponibilidade);
        public Task<Disponibilidade> PostAsync(Disponibilidade disponibilidade);
        public Task DeleteAsync(int id);
    }
}
