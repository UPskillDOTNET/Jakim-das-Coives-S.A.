using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API_Sistema_Central.Services
{
    public class SubAluguerService : ISubAluguerService
    {
        private readonly IReservaService _reservaService;
        private readonly UserManager<Utilizador> _userManager;

        public SubAluguerService(IReservaService reservaService, UserManager<Utilizador> userManager)
        {
            _reservaService = reservaService;
            _userManager = userManager;
        }

        public async Task<ActionResult<IEnumerable<SubAluguerDTO>>> GetByNifAsync(string nif)
        {
            var u = await _userManager.FindByIdAsync(nif);
            if (u == null)
            {
                throw new Exception("O utilizador não existe.");
            }
            try
            {
                IEnumerable<SubAluguerDTO> result;
                using (HttpClient client = new HttpClient())
                {
                    string endpoint = "https://localhost:5005/api/lugares/all/" + nif;
                    var response = await client.GetAsync(endpoint);
                    response.EnsureSuccessStatusCode();
                    result = await response.Content.ReadAsAsync<IEnumerable<SubAluguerDTO>>();
                }
                if (!result.Any())
                {
                    return result.ToList();
                }
                foreach (SubAluguerDTO s in result)
                {
                    bool isReservado = await IsReservado(s.Id);
                    s.IsReservado = isReservado;
                    s.NomeParque = await GetParqueNomeByID(s.ParqueId);
                }
                return result.ToList();
            }
            catch (Exception)
            {
                throw new Exception("Ligação aos Sub-Alugueres falhou.");
            }
        }

        public async Task<ActionResult<SubAluguerDTO>> GetByIdAsync(int id)
        {
            try
            {
                SubAluguerDTO result;
                using (HttpClient client = new HttpClient())
                {
                    string endpoint = "https://localhost:5005/api/lugares/" + id;
                    var response = await client.GetAsync(endpoint);
                    response.EnsureSuccessStatusCode();
                    result = await response.Content.ReadAsAsync<SubAluguerDTO>();
                }
                bool isReservado = await IsReservado(result.Id);
                result.IsReservado = isReservado;
                result.NomeParque = await GetParqueNomeByID(result.ParqueId);
                return result;
            }
            catch (Exception)
            {
                throw new Exception("Este lugar não existe.");
            }
        }

        public async Task<SubAluguerDTO> PostSubAluguerAsync(SubAluguerDTO subAluguerDTO)
        {
            await ValidarSubAluguer(subAluguerDTO);
            subAluguerDTO.ParqueId = await GetParqueIdByNome(subAluguerDTO.NomeParque);
            try
            {
                SubAluguerDTO lugar;
                using (HttpClient client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(subAluguerDTO), Encoding.UTF8, "application/json");
                    string endpoint = "https://localhost:5005/api/lugares";
                    var response = await client.PostAsync(endpoint, content);
                    response.EnsureSuccessStatusCode();
                    lugar = await response.Content.ReadAsAsync<SubAluguerDTO>();
                }
                return lugar;
            }
            catch
            {
                throw new Exception("O registo do lugar para Sub-Aluguer falhou.");
            }
        }

        public async Task DeleteSubAluguerAsync(int id)
        {
            if (IsReservado(id).Result)
            {
                throw new Exception("Proibido: o lugar está reservado.");
            }
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string endpoint = "https://localhost:5005/api/lugares/" + id;
                    var response = await client.DeleteAsync(endpoint);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch
            {
                throw new Exception("O cancelamento do lugar para Sub-Aluguer falhou.");
            }
        }

        private async Task<bool> IsReservado(int lugarId)
        {
            bool isReservado = false;
            var reservas = new List<ReservaAPIParqueDTO>();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = "https://localhost:5005/api/reservas";
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                reservas = await response.Content.ReadAsAsync<List<ReservaAPIParqueDTO>>();
            }
            var r = reservas.Where(x => x.LugarId == lugarId);
            if (r.Any())
            {
                isReservado = true;
            }
            return isReservado;
        }
        private async Task ValidarSubAluguer(SubAluguerDTO subAluguerDTO)
        {
            DetalheReservaDTO r = await _reservaService.GetByIdAsync(subAluguerDTO.ReservaSistemaCentralId);
            if (r == null)
            {
                throw new Exception("Não existe reserva para sub-alugar.");
            }
            if (r.IsSubAlugado == true)
            {
                throw new Exception("Proibido: Este lugar já está Sub-Alugado.");
            }
            if (r.NifProprietario != subAluguerDTO.NifProprietario)
            {
                throw new Exception("Não é o proprietário dessa reserva.");
            }
            if (r.Inicio != subAluguerDTO.Inicio || r.Fim != subAluguerDTO.Fim || r.Andar != subAluguerDTO.Andar || r.Fila != subAluguerDTO.Fila || r.NumeroLugar != subAluguerDTO.Numero)
            {
                throw new Exception("Alguns detalhes do sub-aluguer não estão coerentes.");
            }
            if (subAluguerDTO.Preco <= 0)
            {
                throw new Exception("O preço não pode ser igual ou menor que zero.");
            }
        }
        private static async Task<int> GetParqueIdByNome(string nome)
        {
            try
            {
                ParqueDTO p;
                using (HttpClient client = new HttpClient())
                {
                    string endpoint1 = "https://localhost:5005/api/parques/";
                    var response1 = await client.GetAsync(endpoint1);
                    response1.EnsureSuccessStatusCode();
                    var lista = await response1.Content.ReadAsAsync<List<ParqueDTO>>();
                    p = lista.First(t => t.Rua == nome);
                }
                if (p == null)
                {
                    throw new Exception("Este parque não existe.");
                }
                return p.Id;
            }
            catch (Exception)
            {
                throw new Exception("GetParqueIdByNome() falhou.");
            }
        }
        private static async Task<string> GetParqueNomeByID(int id)
        {
            try
            {
                ParqueDTO p;
                using (HttpClient client = new HttpClient())
                {
                    string endpoint1 = "https://localhost:5005/api/parques/" + id;
                    var response1 = await client.GetAsync(endpoint1);
                    response1.EnsureSuccessStatusCode();
                    p = await response1.Content.ReadAsAsync<ParqueDTO>();
                }
                return p.Rua;
            }
            catch (Exception)
            {
                throw new Exception("GetParqueNomeByID() falhou.");
            }
        }
    }
}
