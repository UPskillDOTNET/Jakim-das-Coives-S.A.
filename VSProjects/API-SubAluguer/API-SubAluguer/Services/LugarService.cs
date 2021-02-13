using API_SubAluguer.DTOs;
using API_SubAluguer.Models;
using API_SubAluguer.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Services
{
    public class LugarService : ILugarService
    {
        private readonly ILugarRepository _repository;
        private readonly IParqueRepository _parqueRepository;
        private readonly IReservaRepository _reservaRepository;
        private readonly IDisponibilidadeRepository _disponibilidadeRepository;

        public LugarService(ILugarRepository repository, IParqueRepository parqueRepository, IReservaRepository reservaRepository, IDisponibilidadeRepository disponibilidadeRepository)
        {
            _repository = repository;
            _parqueRepository = parqueRepository;
            _reservaRepository = reservaRepository;
            _disponibilidadeRepository = disponibilidadeRepository;
        }

        public async Task<ActionResult<IEnumerable<Lugar>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Lugar> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<ActionResult<IEnumerable<Lugar>>> GetByNifAsync(string nif)
        {
            var temp = await _repository.GetAllAsync();
            var lista = temp.Value.Where(t => t.NifProprietario == nif);
            if (!lista.Any())
            {
                throw new Exception("Não existem lugares associados a este NIF.");
            }
            return lista.ToList();
        }

        public async Task<Lugar> PostAsync(SubAluguerDTO subAluguerDTO)
        {
            Lugar l = new Lugar { Numero = subAluguerDTO.Numero, Fila = subAluguerDTO.Fila, Andar = subAluguerDTO.Andar, ParqueId = subAluguerDTO.ParqueId, NifProprietario = subAluguerDTO.NifProprietario, Preco = subAluguerDTO.Preco };
            Lugar lugar = await _repository.PostAsync(l);
            if (lugar == null)
            {
                throw new Exception("O registo do lugar para sub-aluguer falhou.");
            }
            Disponibilidade d = new Disponibilidade { LugarId = lugar.Id, Inicio = subAluguerDTO.Inicio, Fim = subAluguerDTO.Fim };
            Disponibilidade disponibilidade = await _disponibilidadeRepository.PostAsync(d);
            if (disponibilidade == null)
            {
                throw new Exception("O registo da disponibilidade do lugar falhou.");
            }
            return lugar;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public IEnumerable<Lugar> FindAvailable(int freguesiaId, DateTime inicio, DateTime fim)
        {
            List<Lugar> todoslugares = new List<Lugar>();
            var parques = _parqueRepository.GetAllAsync().Result.Value.Where(p => p.FreguesiaId == freguesiaId);
            foreach (Parque p in parques)
            {
                var temp = _disponibilidadeRepository.GetAllIncludeAsync().Result.Value.Where(r => r.Inicio <= inicio && r.Fim >= fim && r.Lugar.ParqueId == p.Id);
                foreach (Disponibilidade d in temp)
                {
                    todoslugares.Add(d.Lugar);
                }
            }

            List<Lugar> ocupados = new List<Lugar>();
            var reservas = _reservaRepository.GetAllAsync().Result.Value.Where(r => 
                r.Fim > inicio && r.Fim <= fim ||
                r.Inicio >= inicio && r.Inicio < fim ||
                r.Inicio >= inicio && r.Fim <= fim ||
                r.Inicio <= inicio && r.Fim >= fim);
            foreach (Reserva r in reservas)
            {
                ocupados.Add(r.Lugar);
            }

            var disponiveis = todoslugares.Except(ocupados);

            return disponiveis;
        }
    }
}
