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
    public class TransacaoService : ITransacaoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;

        public TransacaoService(IHttpContextAccessor httpContextAccessor, UserManager<Utilizador> userManager, SignInManager<Utilizador> signInManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IEnumerable<Transacao>> GetAllTransacoesByNIF()
        {
            string nif;
            try
            {
                nif = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            catch
            {
                throw new Exception("Utilizador não tem login feito.");
            }

            var token = await GetTokenByNIf(nif);

            var listaTransacoes = new List<Transacao>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string endpoint = "https://localhost:5050/api/transacoes/all/" + nif;
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                listaTransacoes = await response.Content.ReadAsAsync<List<Transacao>>();
            }
            return listaTransacoes;
        }

        private async Task<string> GetTokenByNIf(string nif)
        {
            var user = await _userManager.FindByIdAsync(nif);
            if (user.Expiration < DateTime.UtcNow)
            {
                await _signInManager.SignOutAsync();
                throw new Exception("A sua sessão expirou. Volte a autenticar-se.");
            }
            else
            {
                return user.Token;
            }
        }
    }
}