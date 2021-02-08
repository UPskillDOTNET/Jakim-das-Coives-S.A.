using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Models;
using API_Sistema_Central.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Services
{
    public class CartaoService :  ICartaoService
    {
        private readonly ICartaoRepository _repository;

        public CartaoService(ICartaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult<IEnumerable<Cartao>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Cartao> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task PutAsync(Cartao cartao)
        {
            await _repository.PutAsync(cartao);
        }

        public async Task<Cartao> PostAsync(Cartao cartao)
        {
            return await _repository.PostAsync(cartao);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}