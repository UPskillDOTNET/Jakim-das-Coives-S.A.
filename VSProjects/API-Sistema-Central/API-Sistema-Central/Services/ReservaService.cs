using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Models;
using API_Sistema_Central.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Services
{
    public class ReservaService
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

        public async Task PutAsync(Reserva Reserva)
        {
            await _repository.PutAsync(Reserva);
        }

        public async Task<Reserva> PostAsync(Reserva Reserva)
        {
            return await _repository.PostAsync(Reserva);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
