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

        public void Pay(PagamentoDTO payDTO)
        {
            var method = payDTO.MetodoId;

            switch (method)
            {
                case 1:
                    //cartao
                    break;

                case 2:
                    //debito direto
                    break;

                case 3:
                    //paypal
                    break;

                case 4:
                    //carteira
                    break;

                default:
                    throw new IndexOutOfRangeException();
            }
        }

        async void IPagamentoService.PayWithCartao(CartaoDTO dTO)
        {
            MetodoPagamento cartaoMetodo = await _repository.GetByIdAsync(1);
            _client.BaseAddress = new Uri(cartaoMetodo.ApiUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var cartaoURI = "api/Cartoes";

            HttpResponseMessage response = await _client.PostAsJsonAsync(cartaoURI, dTO);
            response.EnsureSuccessStatusCode();
        }

        void IPagamentoService.PayWithCarteira(string nif)
        {
            throw new NotImplementedException();
        }

        async void IPagamentoService.PayWithDebitoDireto(DebitoDiretoDTO dTO)
        {
            MetodoPagamento debitoDiretoMetodo = await _repository.GetByIdAsync(2);
            _client.BaseAddress = new Uri(debitoDiretoMetodo.ApiUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var debitoDiretoURI = "api/DebitosDiretos";

            HttpResponseMessage response = await _client.PostAsJsonAsync(debitoDiretoURI, dTO);
            response.EnsureSuccessStatusCode();
        }

        async void IPagamentoService.PayWithPayPal(PayPalDTO dTO)
        {
            MetodoPagamento payPalMetodo = await _repository.GetByIdAsync(3);
            _client.BaseAddress = new Uri(payPalMetodo.ApiUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var payPalURI = "api/paypal";

            HttpResponseMessage response = await _client.PostAsJsonAsync(payPalURI, dTO);
            response.EnsureSuccessStatusCode();
        }
    }
}
