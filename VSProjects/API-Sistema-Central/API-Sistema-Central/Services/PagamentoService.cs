using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using API_Sistema_Central.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace API_Sistema_Central.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IMetodoPagamentoRepository _repository;
        private HttpClient _client;

        public PagamentoService(IMetodoPagamentoRepository repository)
        {
            _repository = repository;
            _client = new HttpClient();
        }

        async Task<ActionResult> IPagamentoService.PayWithCartao(CartaoDTO dTO)
        {
            MetodoPagamento cartaoMetodo = await _repository.GetByIdAsync(1);
            _client.BaseAddress = new Uri(cartaoMetodo.ApiUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var cartaoURI = "Cartoes";

            try
            {
                var response = _client.PostAsJsonAsync(cartaoURI, dTO);

            } catch
            {
                throw;
            }

            throw new NotImplementedException();
        }

        Task<ActionResult> IPagamentoService.PayWithCarteira(string nif)
        {
            throw new NotImplementedException();
        }

        Task<ActionResult> IPagamentoService.PayWithDebitoDireto(DebitoDiretoDTO dTO)
        {
            throw new NotImplementedException();
        }

        Task<ActionResult> IPagamentoService.PayWithPayPal(PayPalDTO dTO)
        {
            throw new NotImplementedException();
        }
    }
}
