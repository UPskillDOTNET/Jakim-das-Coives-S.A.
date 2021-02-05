using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Models;
using API_Sistema_Central.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Services
{
    public class CartaoService : ICartaoService
    {
        private readonly ICartaoRepository _repository;

        public CartaoService(ICartaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult<IEnumerable<Cartao>>> GetAllCartao()
        {
            return await _repository.GetAllCartao();
        }
    }
}
