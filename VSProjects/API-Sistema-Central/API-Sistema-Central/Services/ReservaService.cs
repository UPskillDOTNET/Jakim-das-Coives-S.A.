using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using API_Sistema_Central.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API_Sistema_Central.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _repository;
        private readonly IParqueRepository _parqueRepository;
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IEmailService _emailService;
        private readonly UserManager<Utilizador> _userManager;

        public ReservaService(IReservaRepository repository, IParqueRepository parqueRepository, ITransacaoRepository transacaoRepository, UserManager<Utilizador> userManager, IEmailService emailService)
        {
            _repository = repository;
            _parqueRepository = parqueRepository;
            _transacaoRepository = transacaoRepository;
            _userManager = userManager;
            _emailService = emailService;
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
                        string endpoint2 = parque.ApiUrl + "api/lugares/disponibilidade/" + f.Id + "/" + inicio.ToString("yyyy-MM-ddTHH:mm:ss") + " / " + fim.ToString("yyyy-MM-ddTHH:mm:ss");
                        var response2 = await client.GetAsync(endpoint2);
                        response2.EnsureSuccessStatusCode();
                        List<LugarDTO> temp = await response2.Content.ReadAsAsync<List<LugarDTO>>();
                        foreach (LugarDTO l in temp)
                        {
                            l.ParqueIdSC = parque.Id;
                            l.ApiUrl = parque.ApiUrl;
                            listaLugares.Add(l);
                        }
                    };
                }
            }
            return listaLugares;   
        }

        public async Task<ActionResult<IEnumerable<Reserva>>> GetByNifAsync(string nif)
        {
            var temp = await _repository.GetAllAsync();
            var lista = temp.Value.Where(t => t.NifUtilizador == nif);
            return lista.ToList();
        }

        public async Task<Reserva> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Reserva> PostAsync(ReservaDTO reservaDTO)
        {
            Reserva reserva = new Reserva { NifUtilizador = reservaDTO.NifUtilizador, ParqueId = reservaDTO.ParqueId };

            //Reservar o lugar na API-Parque
            ReservaAPIParqueDTO r = new ReservaAPIParqueDTO { NifCliente = 999999999, LugarId = reservaDTO.LugarId, Inicio = reservaDTO.Inicio, Fim = reservaDTO.Fim };
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(r), Encoding.UTF8, "application/json");
                string endpoint = reservaDTO.ApiUrl + "api/reservas";
                var response = await client.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
                ReservaAPIParqueDTO r2 = await response.Content.ReadAsAsync<ReservaAPIParqueDTO>();
                reserva.ReservaParqueId = r2.Id;
            }

            //Calcular o custo da Reserva
            var h = (reservaDTO.Fim - reservaDTO.Inicio).TotalHours;
            reserva.Custo = h * reservaDTO.Preco;

            //Fazer pagamento da reserva
            //Utilizador utilizador = await _userManager.FindByIdAsync(reservaDTO.NifUtilizador);

            //Enviar email de confirmacao
            _emailService.EnviarEmail();

            //Registar a transacao do pagamento da reserva
            Transacao transacao = await _transacaoRepository.PostAsync(new Transacao { NifPagador = reservaDTO.NifUtilizador, NifRecipiente = reservaDTO.NifVendedor, Valor = reserva.Custo, MetodoId = reservaDTO.MetodoId, DataHora = DateTime.UtcNow });
            reserva.TransacaoId = transacao.Id;

            return await _repository.PostAsync(reserva);
        }

        public async Task DeleteAsync(int id)
        {
            Reserva reserva = await _repository.GetByIdAsync(id);
            Parque parque = await _parqueRepository.GetByIdAsync(reserva.ParqueId);

            //Apagar a reserva na API-Parque
            using (HttpClient client = new HttpClient())
            {
                string endpoint1 = parque.ApiUrl + "api/reservas/" + reserva.ReservaParqueId;
                var response1 = await client.DeleteAsync(endpoint1);
                response1.EnsureSuccessStatusCode();
            }

            //Reembolsar a carteira do utilizador

            await _repository.DeleteAsync(id);
        }
    }
}