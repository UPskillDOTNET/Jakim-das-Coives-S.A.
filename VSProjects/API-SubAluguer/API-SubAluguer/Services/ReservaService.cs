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

        public async Task<IEnumerable<Reserva>> GetAllAsync()
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
            if (!IsNotAvailable(reserva.LugarId, reserva.Inicio, reserva.Fim))
            {
                return await _repository.PostAsync(reserva);
            }
            else
            {
                throw new Exception("Este lugar não se encontra disponível");
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        private bool IsNotAvailable(int lugarId, DateTime inicio, DateTime fim)
        {
            var reservas = _repository.GetAllAsync().Result;
            return reservas.Any(r =>
                r.LugarId == lugarId &&
                (r.Fim > inicio && r.Fim <= fim ||
                r.Inicio >= inicio && r.Inicio < fim ||
                r.Inicio >= inicio && r.Fim <= fim ||
                r.Inicio <= inicio && r.Fim >= fim));
        }
    }
}
