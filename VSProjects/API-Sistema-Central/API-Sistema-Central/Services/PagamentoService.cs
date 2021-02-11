using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using API_Sistema_Central.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public PagamentoService(IMetodoPagamentoRepository repository, UserManager<Utilizador> userManager) //construtor do serviço
        {
            _repository = repository;
            _userManager = userManager;
            _client = new HttpClient();
        }

        public async Task Pay(PagamentoDTO payDTO) //pagar uma reserva
        {
            var method = payDTO.MetodoId;

            Utilizador payingUser = await _userManager.Users.Include(x => x.Credencial).SingleAsync(x => x.Id == payDTO.NifPagador); //retorna um utilizador com a sua credencial de pagamento incluída

            Credencial userCredentials = payingUser.Credencial;

            switch (method) // id's na base de dados
            {

                /*
                 * 1 - Cartao
                 * 2 - Debito Direto
                 * 3 - PayPal
                 * 4 - Carteira do Sistema Jakim das Coives
                 * 
                 * */



                case 1:
                    {
                        //cartao
                        if (userCredentials is not Cartao) throw new Exception("O utilizador não possui dados para cartão de crédito!"); //conversao de credencial generica para especifica do cartao
                        Cartao convUser = (Cartao)userCredentials;
                        CartaoDTO dto = CartaoDTOBuilder(convUser, int.Parse(payDTO.NifRecipiente), payDTO.Valor);
                        await PayWithCartao(dto);
                        break;
                    }

                case 2:
                    {
                        //debito direto
                        if (userCredentials is not DebitoDireto) throw new Exception("O utilizador não possui dados para débito direto!!");
                        DebitoDireto convUser = (DebitoDireto)userCredentials;
                        DebitoDiretoDTO dto = DebitoDiretoDTOBuilder(convUser, int.Parse(payDTO.NifRecipiente), payDTO.Valor);
                        try
                        {
                            await PayWithDebitoDireto(dto);
                        }
                        catch
                        {
                            throw;
                        }
                        break;
                    }

                case 3:
                    {
                        //paypal
                        if (userCredentials is not PayPal) throw new Exception("O utilizador não possui dados para PayPal!");
                        PayPal convUser = (PayPal)userCredentials;
                        Utilizador receivingUser = await _userManager.FindByIdAsync(payDTO.NifRecipiente);
                        string receiverEmail = receivingUser.Email;
                        PayPalDTO dto = PayPalDTOBuilder(convUser, receiverEmail, payDTO.Valor);
                        try
                        {
                            await PayWithPayPal(dto);
                        }
                        catch
                        {
                            throw;
                        }
                        break;
                    }

                case 4:
                    {
                        //carteira
                        if (payingUser.Carteira - payDTO.Valor < 0) throw new Exception("O utilizador não tem dinheiro suficiente na carteira.");
                        else
                        {
                            Utilizador receivingUser = await _userManager.FindByIdAsync(payDTO.NifRecipiente);
                            await PayWithCarteira(payingUser, receivingUser, payDTO.Valor);
                        }
                        break;
                    }

                default:
                    throw new Exception("Método de Pagamento Não Suportado!"); // metodo ainda nao implementado
            }
        }

        public async Task Reembolso(Transacao transacao)
        {
            Utilizador reembolsadoUser = await _userManager.FindByIdAsync(transacao.NifPagador); //identificar users para lhes mexer na carteira
            Utilizador provedorUser = await _userManager.FindByIdAsync(transacao.NifRecipiente);
            double valor = transacao.Valor;

            double rUOriginal = reembolsadoUser.Carteira;
            double pUOriginal = provedorUser.Carteira;
            try
            {
                reembolsadoUser.Carteira += valor;
                provedorUser.Carteira -= valor;
                await _userManager.UpdateAsync(reembolsadoUser);
                await _userManager.UpdateAsync(provedorUser);
            }
            catch
            {
                reembolsadoUser.Carteira = pUOriginal; //reverter alterações
                provedorUser.Carteira = rUOriginal;
                throw;
            }
        }

        #region Payment Methods

        private async Task PayWithCartao(CartaoDTO dTO)
        {
            MetodoPagamento cartaoMetodo = await _repository.GetByIdAsync(1);
            _client.BaseAddress = new Uri(cartaoMetodo.ApiUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var cartaoURI = "api/cartoes";

            HttpResponseMessage response = await _client.PostAsJsonAsync(cartaoURI, dTO);
            response.EnsureSuccessStatusCode();
        }

        private async Task PayWithCarteira(Utilizador payingUser, Utilizador receivingUser, double valor)
        {
            double pUOriginal = payingUser.Carteira;
            double rUOriginal = receivingUser.Carteira;
            try
            {
                payingUser.Carteira -= valor;
                receivingUser.Carteira += valor;
                await _userManager.UpdateAsync(payingUser);
                await _userManager.UpdateAsync(receivingUser);
            }
            catch
            {
                payingUser.Carteira = pUOriginal;
                receivingUser.Carteira = rUOriginal;
                throw;
            }
        }

        private async Task PayWithDebitoDireto(DebitoDiretoDTO dTO)
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

        private async Task PayWithPayPal(PayPalDTO dTO)
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

        private PayPalDTO PayPalDTOBuilder(PayPal pCred, string emailDest, double custo)
        {
            PayPalDTO createdDTO = new PayPalDTO
            {
                Email = pCred.Password,
                Custo = custo,
                Password = pCred.Password,
                EmailDestinatario = emailDest,
                Data = DateTime.Now
            };
            return createdDTO;
        }

        #endregion
    }
}
