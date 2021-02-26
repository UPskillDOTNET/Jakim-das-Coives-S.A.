using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace APP_FrontEnd.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransacaoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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

            var listaTransacoes = new List<Transacao>();
            using (HttpClient client = new HttpClient())
            {
                string endpoint1 = "https://localhost:5050/api/transacoes/all/" + nif;
                var response1 = await client.GetAsync(endpoint1);
                response1.EnsureSuccessStatusCode();
                listaTransacoes = await response1.Content.ReadAsAsync<List<Transacao>>();
            }
            return listaTransacoes;
        }
    }
}