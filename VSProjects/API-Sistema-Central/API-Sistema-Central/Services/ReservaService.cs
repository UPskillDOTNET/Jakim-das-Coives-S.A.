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

        public ReservaService(IReservaRepository repository, IParqueRepository parqueRepository)
        {
            _repository = repository;
            _parqueRepository = parqueRepository;
        }

        public async Task<ActionResult<IEnumerable<LugarDTO>>> FindAvailableAsync(string freguesiaNome, DateTime inicio, DateTime fim)
        {
            var listaLugares = new List<LugarDTO>();
            var listaParques = _parqueRepository.GetAllAsync().Result;
            foreach (Parque parque in listaParques.Value)
            {
                using (HttpClient client = new HttpClient())
                {
                    var listaFreguesias = new List<FreguesiaDTO>();
                    string endpoint1 = parque.ApiUrl + "api/freguesias";
                    var response1 = await client.GetAsync(endpoint1);
                    response1.EnsureSuccessStatusCode();
                    listaFreguesias = await response1.Content.ReadAsAsync<List<FreguesiaDTO>>();
                    FreguesiaDTO f = listaFreguesias.Find(z => z.Nome == freguesiaNome);
                    if (f != null)
                    {
                        string endpoint2 = parque.ApiUrl + "api/lugares/disponibilidade/" + f.Id + "/" + inicio.ToString("yyyy-MM-ddThh:mm:ss") + " / " + fim.ToString("yyyy-MM-ddThh:mm:ss");
                        var response2 = await client.GetAsync(endpoint2);
                        response2.EnsureSuccessStatusCode();
                        List<LugarDTO> temp = await response2.Content.ReadAsAsync<List<LugarDTO>>();
                        foreach (LugarDTO l in temp)
                        {
                            l.ApiUrl = parque.ApiUrl;
                            listaLugares.Add(l);
                        }
                    };
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