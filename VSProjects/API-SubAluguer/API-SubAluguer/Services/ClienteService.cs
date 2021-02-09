using API_SubAluguer.Models;
using API_SubAluguer.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult<IEnumerable<Cliente>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task PutAsync(Cliente cliente)
        {
            await _repository.PutAsync(cliente);
        }

        public async Task<Cliente> PostAsync(Cliente cliente)
        {
            return await _repository.PostAsync(cliente);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
