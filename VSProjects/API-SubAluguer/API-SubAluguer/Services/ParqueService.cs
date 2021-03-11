using API_SubAluguer.Models;
using API_SubAluguer.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Services
{
    public class ParqueService : IParqueService
    {
        private readonly IParqueRepository _repository;

        public ParqueService(IParqueRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Parque>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Parque> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task PutAsync(Parque parque)
        {
            await _repository.PutAsync(parque);
        }

        public async Task<Parque> PostAsync(Parque parque)
        {
            return await _repository.PostAsync(parque);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
