using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Models;
using API_Sistema_Central.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Services
{
    public class TransacaoService :  ITransacaoService
    {
        private readonly ITransacaoRepository _repository;

        public TransacaoService(ITransacaoRepository repository)
        {
            _repository = repository;
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
            var temp = await _repository.GetAllAsync();
            var lista = temp.Value.Where(t => t.NifPagador == nif || t.NifRecipiente == nif);
            if (!lista.Any())
            {
                throw new Exception("Não existem transações associadas a este NIF.");
            }
            return lista.ToList();
        }
    }
}