using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Models;
using API_Sistema_Central.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Services
{
    public class TransacaoService :  ITransacaoService
    {
        private readonly ITransacaoRepository _repository;
        private readonly UserManager<Utilizador> _userManager;

        public TransacaoService(ITransacaoRepository repository, UserManager<Utilizador> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<Transacao> GetByIdAsync(int id)
        {
            Transacao t = await _repository.GetByIdAsync(id);
            if (t == null)
            {
                throw new Exception("Esta transação não existe.");
            }
            return t;
        }

        public async Task<ActionResult<IEnumerable<Transacao>>> GetByNifAsync(string nif)
        {
            var u = await _userManager.FindByIdAsync(nif);
            if (u == null)
            {
                throw new Exception("O utilizador não existe.");
            }
            var temp = await _repository.GetAllAsync();
            var lista = temp.Value.Where(t => t.NifPagador == nif || t.NifRecipiente == nif);
            return lista.ToList();
        }
    }
}