using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace APP_FrontEnd.Services
{
    public interface ITransacaoService
    {
        public Task<IEnumerable<TransacaoDTO>> GetAllTransacoesByNIF();
    }

    public class TransacaoService : ITransacaoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<Utilizador> _signInManager;
        private readonly ITokenService _tokenService;

        public TransacaoService(IHttpContextAccessor httpContextAccessor, SignInManager<Utilizador> signInManager, ITokenService tokenService)
        {
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<IEnumerable<TransacaoDTO>> GetAllTransacoesByNIF()
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

            var listaTransacoes = new List<TransacaoDTO>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string endpoint = "https://jakim-api-management.azure-api.net/sistema-central/api/transacoes/all/" + nif;
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                listaTransacoes = await response.Content.ReadAsAsync<List<TransacaoDTO>>();
            }
            return listaTransacoes;
        }
    }
}