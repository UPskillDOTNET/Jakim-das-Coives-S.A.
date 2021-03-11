using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Text;

namespace APP_FrontEnd.Services
{
    public interface IUtilizadorService
    {
        public Task<SaldoDTO> GetSaldoAsync();
        public Task DepositarSaldoAsync(DepositarDTO depositar);
    }

    public class UtilizadorService : IUtilizadorService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<Utilizador> _signInManager;
        private readonly ITokenService _tokenService;

        public UtilizadorService(IHttpContextAccessor httpContextAccessor, SignInManager<Utilizador> signInManager, ITokenService tokenService)
        {
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<SaldoDTO> GetSaldoAsync()
        {
            string nif;
            try
            {
                nif = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            catch
            {
                throw new Exception("Utilizador não tem sessão iniciada.");
            }

            string token;
            try
            {
                token = await _tokenService.GetTokenAsync();
            }
            catch (Exception e)
            {
                await _signInManager.SignOutAsync();
                throw new Exception(e.Message);
            }

            var saldo = new SaldoDTO();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string endpoint = "https://localhost:5050/api/utilizadores/saldo/" + nif;
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                var valor = await response.Content.ReadAsAsync<double>();
                saldo.Valor = Math.Round(valor, 2);
            }
            return saldo;
        }
        public async Task DepositarSaldoAsync(DepositarDTO depositar)
        {
            string nif;
            try
            {
                nif = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            catch
            {
                throw new Exception("Utilizador não tem sessão iniciada.");
            }

            string token;
            try
            {
                token = await _tokenService.GetTokenAsync();
            }
            catch (Exception e)
            {
                await _signInManager.SignOutAsync();
                throw new Exception(e.Message);
            }

            depositar.Nif = nif;

            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(depositar), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string endpoint = "https://localhost:5050/api/utilizadores/depositar";
                var response = await client.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
