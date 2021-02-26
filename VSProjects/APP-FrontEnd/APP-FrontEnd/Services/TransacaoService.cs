using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APP_FrontEnd.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly UserManager<Utilizador> _userManager;

        public TransacaoService (UserManager<Utilizador> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<Transacao>> GetAllTransacoesByNIF(string nif)
        {
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