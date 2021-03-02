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
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace APP_FrontEnd.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;

        public ReservaService(IHttpContextAccessor httpContextAccessor, UserManager<Utilizador> userManager, SignInManager<Utilizador> signInManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IEnumerable<LugarDTO>> FindAvailableAsync(string freguesiaNome, DateTime inicio, DateTime fim)
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

            var token = await GetTokenByNif(nif);

            var listaLugares = new List<LugarDTO>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string endpoint = "https://localhost:5050/api/reservas/disponibilidade/" + freguesiaNome + "/" + inicio.ToString("yyyy-MM-ddTHH:mm:ss") + "/" + fim.ToString("yyyy-MM-ddTHH:mm:ss");
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                listaLugares = await response.Content.ReadAsAsync<List<LugarDTO>>();
            }
            return listaLugares;

        }

        public async Task<IEnumerable<DetalheReservaDTO>> GetByNifAsync()
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

            var token = await GetTokenByNif(nif);

            var listaReservas = new List<DetalheReservaDTO>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string endpoint = "https://localhost:5050/api/reservas/all/" + nif;
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                listaReservas = await response.Content.ReadAsAsync<List<DetalheReservaDTO>>();
            }
            return listaReservas;
        }

        public async Task<DetalheReservaDTO> GetByIdAsync(int id)
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

            var token = await GetTokenByNif(nif);

            var result = new DetalheReservaDTO();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string endpoint = "https://localhost:5050/api/reservas/" + id;
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadAsAsync<DetalheReservaDTO>();
            }
            return result;
        }

        public async Task PostAsync(ReservaDTO reservaDTO)
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

            reservaDTO.NifComprador = nif;

            var token = await GetTokenByNif(nif);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string endpoint = "https://localhost:5050/api/reservas/";
                var response = await client.PostAsJsonAsync(endpoint, reservaDTO);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task DeleteAsync(int id)
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

            var token = await GetTokenByNif(nif);

            var result = new DetalheReservaDTO();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string endpoint = "https://localhost:5050/api/reservas/" + id;
                var response = await client.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadAsAsync<DetalheReservaDTO>();
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