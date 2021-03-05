using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using API_Sistema_Central.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Sistema_Central.Services
{
    public class UtilizadorService : IUtilizadorService
    {
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;
        private readonly ICartaoRepository _cartaoRepository;
        private readonly IDebitoDiretoRepository _debitoDiretoRepository;
        private readonly IPayPalRepository _payPalRepository;
        private readonly IPagamentoService _pagamentoService;
        private readonly ITransacaoRepository _transacaoRepository;

        public UtilizadorService (UserManager<Utilizador> userManager, SignInManager<Utilizador> signInManager, ICartaoRepository cartaoRepository, IDebitoDiretoRepository debitoDiretoRepository, IPayPalRepository payPalRepository, IPagamentoService pagamentoService, ITransacaoRepository transacaoRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cartaoRepository = cartaoRepository;
            _debitoDiretoRepository = debitoDiretoRepository;
            _payPalRepository = payPalRepository;
            _pagamentoService = pagamentoService;
            _transacaoRepository = transacaoRepository;
        }

        public async Task<IdentityResult> RegistarUtilizador(RegistarUtilizadorDTO registarUtilizadorDTO)
        {
            int credencialId;
            switch (registarUtilizadorDTO.MetodoId)
            {
                case 1:
                    Cartao c = new Cartao { MetodoId = registarUtilizadorDTO.MetodoId, Nome = registarUtilizadorDTO.NomeCartao, Numero = registarUtilizadorDTO.Numero, Cvv = registarUtilizadorDTO.Cvv, DataValidade = registarUtilizadorDTO.DataValidade };
                    if (c.Numero == null || c.Nome == null || c.Cvv == null || c.DataValidade == null)
                    {
                        return IdentityResult.Failed();
                    }
                    Cartao cartao = await _cartaoRepository.PostAsync(c);
                    credencialId = cartao.Id;
                    break;
                case 2:
                    DebitoDireto d = new DebitoDireto { MetodoId = registarUtilizadorDTO.MetodoId, Iban = registarUtilizadorDTO.Iban, CodigoPostal = registarUtilizadorDTO.CodigoPostal, Freguesia = registarUtilizadorDTO.Freguesia, Nome = registarUtilizadorDTO.NomeDebitoDireto, DataSubscricao = registarUtilizadorDTO.DataSubscricao, Rua = registarUtilizadorDTO.Rua };
                    if ( d.Iban == null || d.CodigoPostal == null || d.Freguesia == null || d.Rua == null || d.Nome == null)
                    {
                        return IdentityResult.Failed();
                    }
                    DebitoDireto debitoDireto = await _debitoDiretoRepository.PostAsync(d);
                    credencialId = debitoDireto.Id;
                    break;
                case 3:
                    PayPal p = new PayPal { MetodoId = registarUtilizadorDTO.MetodoId, Email = registarUtilizadorDTO.EmailPayPal, Password = registarUtilizadorDTO.PasswordPayPal };
                    if (p.Password == null || p.Email == null)
                    {
                        return IdentityResult.Failed();
                    }
                    PayPal payPal = await _payPalRepository.PostAsync(p);
                    credencialId = payPal.Id;
                    break;
                default:
                    return IdentityResult.Failed();
            }

            var user = new Utilizador { Id = registarUtilizadorDTO.Nif, UserName = registarUtilizadorDTO.EmailUtilizador, Nome = registarUtilizadorDTO.NomeUtilizador, Email = registarUtilizadorDTO.EmailUtilizador, CredencialId = credencialId };
            var result = await _userManager.CreateAsync(user, registarUtilizadorDTO.PasswordUtilizador);
            return result;
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> Login(InfoUtilizadorDTO infoUtilizadorDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(infoUtilizadorDTO.Email, infoUtilizadorDTO.Password, isPersistent: false, lockoutOnFailure: false);
            return result;
        }

        public async Task<double> GetSaldoAsync(string nif)
        {
            Utilizador utilizador = await _userManager.FindByIdAsync(nif);
            if (utilizador != null)
            {
                double saldo = utilizador.Carteira;
                return saldo;
            }
            else
            {
                throw new Exception("Este utilizador não existe.");
            }
        }

        public async Task DepositarSaldoAsync(string nif, double valor)
        {
            Utilizador utilizador = await _userManager.Users.Include(x => x.Credencial).SingleAsync(x => x.Id == nif);
            if (utilizador != null)
            {
                PagamentoDTO deposito = new PagamentoDTO { MetodoId = utilizador.Credencial.MetodoId, NifPagador = nif, NifRecipiente = nif, Valor = valor };
                await _pagamentoService.Pay(deposito);
                await _transacaoRepository.PostAsync(new Transacao { NifPagador = nif, NifRecipiente = nif, Valor = valor, MetodoId = utilizador.Credencial.MetodoId, DataHora = DateTime.UtcNow, Tipo = Tipo.Deposito });
            }
            else
            {
                throw new Exception("Este utilizador não existe.");
            }
        }

        public async Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            Utilizador utilizador = await _userManager.FindByIdAsync(resetPasswordDTO.Nif);
            if (utilizador != null)
            {
                await _userManager.RemovePasswordAsync(utilizador);
                await _userManager.AddPasswordAsync(utilizador, resetPasswordDTO.Password);
            }
            else
            {
                throw new Exception("Este utilizador não existe.");
            }
        }
    }
}
