using API_SubAluguer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Services
{
    public interface IReservaService
    {
        public Task<IEnumerable<Reserva>> GetAllAsync();
        public Task<Reserva> GetByIdAsync(int id);
        public Task PutAsync(Reserva reserva);
        public Task<Reserva> PostAsync(Reserva reserva);
        public Task DeleteAsync(int id);
    }
}