using APICartao.Models;
using APICartao.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICartao.Services
{
    public class CartaoService
    {
        private readonly ICartaoRepository _repo;

        public CartaoService(ICartaoRepository repo) => this._repo = repo;

        public async Task<ActionResult<IEnumerable<Cartao>>> GetAllCartoes()
        {
            return await this._repo.FindAllAsync();
        }

        public async Task<ActionResult<Cartao>> GetCartaoById(long id)
        {
            return await _repo.FindById(id);
        }

        public async Task<ActionResult<IEnumerable<Cartao>>> GetCartoesByNumero(string n)
        {
            return await _repo.GetCartoesByNumeroAsync(n);
        }

        public async Task<ActionResult<Cartao>> CreateNewCartao(Cartao c)
        {
            return await _repo.Create(c);
        }
    }
}
