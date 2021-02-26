using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace APP_FrontEnd.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransacaoService (IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Transacao>> GetAllTransacoesByNIF()
        {

            var nif = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

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