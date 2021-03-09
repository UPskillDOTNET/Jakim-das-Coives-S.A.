using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API_Sistema_Central.Authentication;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using API_Sistema_Central.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API_Sistema_Central.Services
{
    public interface IUtilizadorService
    {
        public Task<TokenResponse> RegistarUtilizador(RegistarUtilizadorDTO registarUtilizadorDTO, string ipAddress);
        public Task<TokenResponse> Login(InfoUtilizadorDTO infoUtilizadorDTO, string ipAddress);
        public Task<TokenResponse> RefreshTokenAsync(string token, string ipAddress);
        public Task<bool> RevokeTokenAsync(string token, string ipAddress);
        public Task<double> GetSaldoAsync(string nif);
        public Task DepositarSaldoAsync(string nif, double valor);
        public Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        public Task AlterarPasswordAsync(AlterarPasswordDTO alterarPasswordDTO);
        public Task AlterarNomeAsync(AlterarNomeDTO alterarNomeDTO);
        public Task AlterarMetodoPagamentoAsync(AlterarMetodoPagamentoDTO alterarMetodoPagamentoDTO);
    }

    public class UtilizadorService : IUtilizadorService
    {
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;
        private readonly ICartaoRepository _cartaoRepository;
        private readonly IDebitoDiretoRepository _debitoDiretoRepository;
        private readonly IPayPalRepository _payPalRepository;
        private readonly IPagamentoService _pagamentoService;
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly AppSettings _appSettings;

        public UtilizadorService(UserManager<Utilizador> userManager, SignInManager<Utilizador> signInManager, ICartaoRepository cartaoRepository, IDebitoDiretoRepository debitoDiretoRepository, IPayPalRepository payPalRepository, IPagamentoService pagamentoService, ITransacaoRepository transacaoRepository, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cartaoRepository = cartaoRepository;
            _debitoDiretoRepository = debitoDiretoRepository;
            _payPalRepository = payPalRepository;
            _pagamentoService = pagamentoService;
            _transacaoRepository = transacaoRepository;
            _appSettings = appSettings.Value;
        }

        public async Task<TokenResponse> RegistarUtilizador(RegistarUtilizadorDTO registarUtilizadorDTO, string ipAddress)
        {
            int credencialId;
            switch (registarUtilizadorDTO.MetodoId)
            {
                case 1:
                    Cartao c = new Cartao { MetodoId = registarUtilizadorDTO.MetodoId, Nome = registarUtilizadorDTO.NomeCartao, Numero = registarUtilizadorDTO.Numero, Cvv = registarUtilizadorDTO.Cvv, DataValidade = registarUtilizadorDTO.DataValidade };
                    if (c.Numero == null || c.Nome == null || c.Cvv == null || c.DataValidade == null)
                    {
                        return null;
                    }
                    Cartao cartao = await _cartaoRepository.PostAsync(c);
                    credencialId = cartao.Id;
                    break;
                case 2:
                    DebitoDireto d = new DebitoDireto { MetodoId = registarUtilizadorDTO.MetodoId, Iban = registarUtilizadorDTO.Iban, CodigoPostal = registarUtilizadorDTO.CodigoPostal, Freguesia = registarUtilizadorDTO.Freguesia, Nome = registarUtilizadorDTO.NomeDebitoDireto, DataSubscricao = registarUtilizadorDTO.DataSubscricao, Rua = registarUtilizadorDTO.Rua };
                    if (d.Iban == null || d.CodigoPostal == null || d.Freguesia == null || d.Rua == null || d.Nome == null)
                    {
                        return null;
                    }
                    DebitoDireto debitoDireto = await _debitoDiretoRepository.PostAsync(d);
                    credencialId = debitoDireto.Id;
                    break;
                case 3:
                    PayPal p = new PayPal { MetodoId = registarUtilizadorDTO.MetodoId, Email = registarUtilizadorDTO.EmailPayPal, Password = registarUtilizadorDTO.PasswordPayPal };
                    if (p.Password == null || p.Email == null)
                    {
                        return null;
                    }
                    PayPal payPal = await _payPalRepository.PostAsync(p);
                    credencialId = payPal.Id;
                    break;
                default:
                    return null;
            }

            var utilizador = new Utilizador { Id = registarUtilizadorDTO.Nif, UserName = registarUtilizadorDTO.EmailUtilizador, Nome = registarUtilizadorDTO.NomeUtilizador, Email = registarUtilizadorDTO.EmailUtilizador, CredencialId = credencialId };
            var result = await _userManager.CreateAsync(utilizador, registarUtilizadorDTO.PasswordUtilizador);

            if (result.Succeeded)
            {
                var jwtToken = GenerateJwtToken(new InfoUtilizadorDTO { Email = registarUtilizadorDTO.EmailUtilizador, Password = registarUtilizadorDTO.PasswordUtilizador });
                var refreshToken = GenerateRefreshToken(ipAddress);

                var user = await _userManager.FindByEmailAsync(registarUtilizadorDTO.EmailUtilizador);
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);

                return new TokenResponse { Token = jwtToken, Nif = user.Id, RefreshToken = refreshToken.Token };
            }
            else
            {
                return null;
            }
        }

        public async Task<TokenResponse> Login(InfoUtilizadorDTO infoUtilizadorDTO, string ipAddress)
        {
            var result = await _signInManager.PasswordSignInAsync(infoUtilizadorDTO.Email, infoUtilizadorDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {

                var jwtToken = GenerateJwtToken(infoUtilizadorDTO);
                var refreshToken = GenerateRefreshToken(ipAddress);

                var user = await _userManager.FindByEmailAsync(infoUtilizadorDTO.Email);
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);

                return new TokenResponse { Token = jwtToken, Nif = user.Id, RefreshToken = refreshToken.Token };
            }
            else
            {
                return null;
            }
        }

        public async Task<TokenResponse> RefreshTokenAsync(string token, string ipAddress)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                return null;
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
            {
                return null;
            }

            var newRefreshToken = GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);

            await _userManager.UpdateAsync(user);

            var jwtToken = GenerateJwtToken(new InfoUtilizadorDTO { Email = user.Email });

            return new TokenResponse { Token = jwtToken, Nif = user.Id, RefreshToken = refreshToken.Token };
        }

        public async Task<bool> RevokeTokenAsync(string token, string ipAddress)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                return false;
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
            {
                return false;
            }

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            await _userManager.UpdateAsync(user);

            return true;
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

        public async Task AlterarPasswordAsync(AlterarPasswordDTO alterarPasswordDTO)
        {
            Utilizador utilizador = await _userManager.FindByIdAsync(alterarPasswordDTO.Nif);
            if (utilizador != null)
            {
                await _userManager.ChangePasswordAsync(utilizador, alterarPasswordDTO.PasswordActual, alterarPasswordDTO.PasswordNova);
            }
            else
            {
                throw new Exception("Este utilizador não existe.");
            }
        }

        public async Task AlterarNomeAsync(AlterarNomeDTO alterarNomeDTO)
        {
            Utilizador utilizador = await _userManager.FindByIdAsync(alterarNomeDTO.Nif);
            if (utilizador != null && utilizador.Nome == alterarNomeDTO.NomeActual)
            {
                utilizador.Nome = alterarNomeDTO.NomeNovo;
                await _userManager.UpdateAsync(utilizador);
            }
            else
            {
                throw new Exception("Este utilizador não existe.");
            }
        }

        public async Task AlterarMetodoPagamentoAsync(AlterarMetodoPagamentoDTO dto)
        {
            var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var utilizador = await _userManager.Users.Include(x => x.Credencial).SingleAsync(x => x.Email == dto.Email);
                if (utilizador == null)
                {
                    throw new Exception("Não existe um utilizador com este email.");
                }
                int credencialId;
                try
                {
                    switch (dto.MetodoId)
                    {
                        case 1:
                            Cartao c = new Cartao { MetodoId = dto.MetodoId, Nome = dto.NomeCartao, Numero = dto.Numero, Cvv = dto.Cvv, DataValidade = dto.DataValidade };
                            if (c.Numero == null || c.Nome == null || c.Cvv == null || c.DataValidade == null)
                            {
                                throw new Exception();
                            }
                            Cartao cartao = await _cartaoRepository.PostAsync(c);
                            credencialId = cartao.Id;
                            break;
                        case 2:
                            DebitoDireto d = new DebitoDireto { MetodoId = dto.MetodoId, Iban = dto.Iban, CodigoPostal = dto.CodigoPostal, Freguesia = dto.Freguesia, Nome = dto.NomeDebitoDireto, DataSubscricao = dto.DataSubscricao, Rua = dto.Rua };
                            if (d.Iban == null || d.CodigoPostal == null || d.Freguesia == null || d.Rua == null || d.Nome == null)
                            {
                                throw new Exception();
                            }
                            DebitoDireto debitoDireto = await _debitoDiretoRepository.PostAsync(d);
                            credencialId = debitoDireto.Id;
                            break;
                        case 3:
                            PayPal p = new PayPal { MetodoId = dto.MetodoId, Email = dto.EmailPayPal, Password = dto.PasswordPayPal };
                            if (p.Password == null || p.Email == null)
                            {
                                throw new Exception();
                            }
                            PayPal payPal = await _payPalRepository.PostAsync(p);
                            credencialId = payPal.Id;
                            break;
                        default:
                            throw new Exception();
                    }
                }
                catch
                {
                    throw new Exception("Registo do novo método de pagamento falhou.");
                }
                int credencialIdAntiga = utilizador.CredencialId;
                int metodoIdAntigo = utilizador.Credencial.MetodoId;
                try
                {
                    utilizador.CredencialId = credencialId;
                    await _userManager.UpdateAsync(utilizador);
                }
                catch
                {
                    throw new Exception("Actualização da credencial do utilizador falhou.");
                }
                try
                {
                    if (metodoIdAntigo == 1)
                    {
                        await _cartaoRepository.DeleteAsync(credencialIdAntiga);
                    }
                    if (metodoIdAntigo == 2)
                    {
                        await _debitoDiretoRepository.DeleteAsync(credencialIdAntiga);
                    }
                    if (metodoIdAntigo == 3)
                    {
                        await _payPalRepository.DeleteAsync(credencialIdAntiga);
                    }
                }
                catch
                {
                    utilizador.CredencialId = credencialIdAntiga;
                    await _userManager.UpdateAsync(utilizador);
                    throw new Exception("Remover antigo método de pagamento falhou.");
                }

            }
        }

        private string GenerateJwtToken(InfoUtilizadorDTO infoUtilizadorDTO)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, infoUtilizadorDTO.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private static RefreshToken GenerateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
    }
}
