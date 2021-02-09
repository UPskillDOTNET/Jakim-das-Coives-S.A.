using API_SubAluguer.Models;
using API_SubAluguer.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Services
{
    public class LugarService : ILugarService
    {
        private readonly ILugarRepository _repository;

        public LugarService(ILugarRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult<IEnumerable<Lugar>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Lugar> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task PutAsync(Lugar lugar)
        {
            await _repository.PutAsync(lugar);
        }

        public async Task<Lugar> PostAsync(Lugar lugar)
        {
            return await _repository.PostAsync(lugar);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
