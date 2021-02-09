using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using API_Sistema_Central.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _repository;
        private readonly IParqueRepository _parqueRepository;
        private readonly HttpClient _client;

        public ReservaService(IReservaRepository repository)
        {
            _repository = repository;
            _client = new HttpClient();
        }

        public async Task<IEnumerable<LugarDTO>> FindAvailableAsync(string freguesiaNome, DateTime inicio, DateTime fim)
        {
            var listaLugares = new List<LugarDTO>();
            Parque parque = _parqueRepository.GetByIdAsync(4).Result;
            using (_client)
            {
                var listaFreguesias = new List<FreguesiaDTO>();
                string endpoint1 = parque.ApiUrl + "freguesias";
                var response1 = await _client.GetAsync(endpoint1);
                response1.EnsureSuccessStatusCode();
                listaFreguesias = await response1.Content.ReadAsAsync<List<FreguesiaDTO>>();
                FreguesiaDTO f = listaFreguesias.Find(z => z.Nome == freguesiaNome);
                /*if (f == null)
                {
                    break;
                }*/

                string endpoint2 = parque.ApiUrl + "lugares/disponibilidade/" + f.Id + "/" + inicio + "/" + fim;
                var response2 = await _client.GetAsync(endpoint2);
                response2.EnsureSuccessStatusCode();
                listaLugares = await response2.Content.ReadAsAsync<List<LugarDTO>>();
                foreach (LugarDTO l in listaLugares)
                {
                    l.ApiUrl = parque.ApiUrl;
                }
            }
            return listaLugares;   
        }

        public async Task<ActionResult<IEnumerable<Reserva>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Reserva> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task PutAsync(Reserva reserva)
        {
            await _repository.PutAsync(reserva);
        }

        public async Task<Reserva> PostAsync(Reserva reserva)
        {
            return await _repository.PostAsync(reserva);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}