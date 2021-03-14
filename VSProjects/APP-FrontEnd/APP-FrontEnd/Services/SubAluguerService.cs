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
    public interface ISubAluguerService
    {
        public Task<IEnumerable<SubAluguerDTO>> GetAllSubAluguerByNIF();
        public Task<SubAluguerDTO> GetSubAluguerById(int id);
        public Task PostSubAluguerAsync(SubAluguerDTO subAluguerDTO);
        public Task DeleteSubAluguerAsync(int id);
    }

    public class SubAluguerService : ISubAluguerService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<Utilizador> _signInManager;
        private readonly ITokenService _tokenService;

        public SubAluguerService(IHttpContextAccessor httpContextAccessor, SignInManager<Utilizador> signInManager, ITokenService tokenService)
        {
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _tokenService = tokenService;
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

            var listaSubAlugueres = new List<SubAluguerDTO>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string endpoint = "https://jakim-api-management.azure-api.net/sistema-central/api/subalugueres/all/" + nif;
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

            var subAluguer = new SubAluguerDTO();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    string endpoint = "https://jakim-api-management.azure-api.net/sistema-central/api/subalugueres/" + id;
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

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(subAluguerDTO), Encoding.UTF8, "application/json");
                    string endpoint = "https://jakim-api-management.azure-api.net/sistema-central/api/subalugueres";
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

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aee2fb676a2e4b25a819af617eb64174");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    string endpoint = "https://jakim-api-management.azure-api.net/sistema-central/api/subalugueres/" + id;
                    var response = await client.DeleteAsync(endpoint);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch
            {
                throw new Exception("O cancelamento do lugar para Sub-Aluguer falhou.");
            }
        }
    }
}