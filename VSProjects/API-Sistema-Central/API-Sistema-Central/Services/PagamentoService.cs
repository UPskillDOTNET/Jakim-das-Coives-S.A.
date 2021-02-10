using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using API_Sistema_Central.Repositories;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<Utilizador> _userManager;
        private HttpClient _client;

        public PagamentoService(IMetodoPagamentoRepository repository, UserManager<Utilizador> userManager)
        {
            _repository = repository;
            _userManager = userManager;
            _client = new HttpClient();
        }

        public async void Pay(PagamentoDTO payDTO)
        {
            var method = payDTO.MetodoId;

            Utilizador payingUser = await _userManager.FindByIdAsync(payDTO.NifPagador);

            Credencial userCredentials = payingUser.Credencial;

            switch (method)
            {
                case 1:
                    {
                        //cartao
                        if (userCredentials is not Cartao) throw new InvalidOperationException();
                        Cartao convUser = (Cartao)userCredentials;
                        CartaoDTO dto = CartaoDTOBuilder(convUser, int.Parse(payDTO.NifRecipiente), payDTO.Valor);
                        try
                        {
                            PayWithCartao(dto);
                        }
                        catch
                        {
                            throw;
                        }
                        break;
                    }
                case 2:
                    {
                        //debito direto
                        if (userCredentials is not DebitoDireto) throw new InvalidOperationException();
                        DebitoDireto convUser = (DebitoDireto)userCredentials;
                        DebitoDiretoDTO dto = DebitoDiretoDTOBuilder(convUser, int.Parse(payDTO.NifRecipiente), payDTO.Valor);
                        try
                        {
                            PayWithDebitoDireto(dto);
                        }
                        catch
                        {
                            throw;
                        }
                        break;
                    }
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

        #region Payment Methods

        public async void PayWithCartao(CartaoDTO dTO)
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

        public void PayWithCarteira(string nif)
        {
            throw new NotImplementedException();
        }

        public async void PayWithDebitoDireto(DebitoDiretoDTO dTO)
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

        public async void PayWithPayPal(PayPalDTO dTO)
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

        #endregion

        #region DTO Builders

        private CartaoDTO CartaoDTOBuilder (Cartao cCred, int nifDest, double custo)
        {
            CartaoDTO createdDTO = new CartaoDTO
            {
                Numero = cCred.Numero,
                Nome = cCred.Nome,
                DataValidade = cCred.DataValidade,
                Cvv = cCred.Cvv,
                Custo = custo,
                NifDestinatario = nifDest,
                Data = DateTime.Now
            };
            return createdDTO;
        }

        private DebitoDiretoDTO DebitoDiretoDTOBuilder(DebitoDireto dDCred, int nifDest, double custo)
        {
            DebitoDiretoDTO createdDTO = new DebitoDiretoDTO
            {
                Iban = dDCred.Iban,
                Nome = dDCred.Nome,
                Rua = dDCred.Rua,
                CodigoPostal = dDCred.CodigoPostal,
                Freguesia = dDCred.Freguesia,
                Custo = custo,
                NifDestinatario = nifDest,
                Data = DateTime.Now
            };
            return createdDTO;
        }

        #endregion



    }
}
