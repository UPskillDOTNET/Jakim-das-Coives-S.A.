using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using API_Sistema_Central.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Services
{
    public interface ITransacaoService
    {
        public Task<Transacao> GetByIdAsync(int id);
        public Task<ActionResult<IEnumerable<TransacaoDTO>>> GetByNifAsync(string nif);
    }

    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacaoRepository _repository;
        private readonly IMetodoPagamentoRepository _metodoPagamentoRepository;
        private readonly UserManager<Utilizador> _userManager;

        public TransacaoService(ITransacaoRepository repository, UserManager<Utilizador> userManager, IMetodoPagamentoRepository metodoPagamentoRepository)
        {
            _repository = repository;
            _userManager = userManager;
            _metodoPagamentoRepository = metodoPagamentoRepository;
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

        public async Task<ActionResult<IEnumerable<TransacaoDTO>>> GetByNifAsync(string nif)
        {
            var lista = new List<TransacaoDTO>();
            var u = await _userManager.FindByIdAsync(nif);
            if (u == null)
            {
                throw new Exception("O utilizador não existe.");
            }
            var temp = await _repository.GetAllAsync();
            var listatemp = temp.Where(t => t.NifPagador == nif || t.NifRecipiente == nif);
            foreach (Transacao t in listatemp)
            {
                var metodo = _metodoPagamentoRepository.GetByIdAsync(t.MetodoId).Result;
                if (t.Tipo == Tipo.Deposito)
                {
                    var dto = new TransacaoDTO { NifDestinatario = t.NifRecipiente, DataHora = t.DataHora, Valor = t.Valor, Tipo = "Depósito", Metodo = metodo.Nome };
                    lista.Add(dto);
                }
                if (t.NifPagador == nif && t.Tipo == Tipo.Reserva)
                {
                    var dto = new TransacaoDTO { NifDestinatario = t.NifRecipiente, DataHora = t.DataHora, Valor = -t.Valor, Tipo = "Reserva", Metodo = metodo.Nome };
                    lista.Add(dto);
                }
                if (t.NifRecipiente == nif && t.Tipo == Tipo.Reserva)
                {
                    var dto = new TransacaoDTO { NifDestinatario = t.NifPagador, DataHora = t.DataHora, Valor = t.Valor, Tipo = "Sub-Aluguer", Metodo = metodo.Nome };
                    lista.Add(dto);
                }
                if (t.NifRecipiente == nif && t.Tipo == Tipo.Reembolso)
                {
                    var dto = new TransacaoDTO { NifDestinatario = t.NifPagador, DataHora = t.DataHora, Valor = t.Valor, Tipo = "Reembolso", Metodo = metodo.Nome };
                    lista.Add(dto);
                }
                if (t.NifPagador == nif && t.Tipo == Tipo.Reembolso)
                {
                    var dto = new TransacaoDTO { NifDestinatario = t.NifRecipiente, DataHora = t.DataHora, Valor = -t.Valor, Tipo = "Devolução", Metodo = metodo.Nome };
                    lista.Add(dto);
                }
            }
            return lista;
        }
    }
}