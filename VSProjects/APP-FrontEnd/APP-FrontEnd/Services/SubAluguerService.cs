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
    public class SubAluguerService : ISubAluguerService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;

        public SubAluguerService(IHttpContextAccessor httpContextAccessor, UserManager<Utilizador> userManager, SignInManager<Utilizador> signInManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IEnumerable<SubAluguerDTO>> GetAllSubAluguerByNIF()
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

            var token = await GetTokenByNif(nif);

            var listaSubAlugueres = new List<SubAluguerDTO>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string endpoint = "https://localhost:5050/api/subalugueres/all/" + nif;
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                listaSubAlugueres = await response.Content.ReadAsAsync<List<SubAluguerDTO>>();
            }
            return listaSubAlugueres;
        }
        public async Task<SubAluguerDTO> GetSubAluguerById(int id)
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

            var token = await GetTokenByNif(nif);

            var subAluguer = new SubAluguerDTO();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    string endpoint = "https://localhost:5050/api/subalugueres/" + id;
                    var response = await client.GetAsync(endpoint);
                    response.EnsureSuccessStatusCode();
                    subAluguer = await response.Content.ReadAsAsync<SubAluguerDTO>();
                }
                return subAluguer;
            }
            catch(Exception)
            {
                throw new Exception("Este lugar não existe.");
            }
        }
        public async Task PostSubAluguerAsync(SubAluguerDTO subAluguerDTO)
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

            var token = await GetTokenByNif(nif);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(subAluguerDTO), Encoding.UTF8, "application/json");
                    string endpoint = "https://localhost:5050/api/subalugueres";
                    var response = await client.PostAsync(endpoint, content);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch
            {
                throw new Exception("O registo do lugar para Sub-Aluguer falhou.");
            }
        }
        public async Task DeleteSubAluguerAsync(int id)
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

            var token = await GetTokenByNif(nif);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    string endpoint = "https://localhost:5050/api/subalugueres/" + id;
                    var response = await client.DeleteAsync(endpoint);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch
            {
                throw new Exception("O cancelamento do lugar para Sub-Aluguer falhou.");
            }
        }
        private async Task<string> GetTokenByNif(string nif)
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