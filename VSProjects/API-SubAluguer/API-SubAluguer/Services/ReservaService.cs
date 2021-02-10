using API_SubAluguer.Models;
using API_SubAluguer.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _repository;

        public ReservaService(IReservaRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult<IEnumerable<Reserva>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Reserva> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task PutAsync(Reserva reserva)
        {
            await _repository.PutAsync(reserva);
        }

        public async Task<Reserva> PostAsync(Reserva reserva)
        {
            return await _repository.PostAsync(reserva);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
