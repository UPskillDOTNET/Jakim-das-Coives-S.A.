using API_SubAluguer.Models;
using API_SubAluguer.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Services
{
    public class DisponibilidadeService : IDisponibilidadeService
    {
        private readonly IDisponibilidadeRepository _repository;

        public DisponibilidadeService(IDisponibilidadeRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult<IEnumerable<Disponibilidade>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Disponibilidade> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task PutAsync(Disponibilidade disponibilidade)
        {
            await _repository.PutAsync(disponibilidade);
        }

        public async Task<Disponibilidade> PostAsync(Disponibilidade disponibilidade)
        {
            return await _repository.PostAsync(disponibilidade);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
