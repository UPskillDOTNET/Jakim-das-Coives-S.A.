using API_SubAluguer.Models;
using API_SubAluguer.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Services
{
    public class FreguesiaService : IFreguesiaService
    {
        private readonly IFreguesiaRepository _repository;

        public FreguesiaService(IFreguesiaRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult<IEnumerable<Freguesia>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Freguesia> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task PutAsync(Freguesia freguesia)
        {
            await _repository.PutAsync(freguesia);
        }

        public async Task<Freguesia> PostAsync(Freguesia freguesia)
        {
            return await _repository.PostAsync(freguesia);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
