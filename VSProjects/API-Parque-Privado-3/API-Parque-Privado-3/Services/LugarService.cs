using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Parque_Privado_3.Models;
using API_Parque_Privado_3.Repositories;

namespace API_Parque_Privado_3.Services
{
    public interface ILugarService
    {
        public Task<IEnumerable<Lugar>> FindAvailableAsync(int freguesiaId, DateTime inicio, DateTime fim);
    }
    public class LugarService : ILugarService
    {
        private readonly ILugarRepository _lugarRepository;
        private readonly IParqueRepository _parqueRepository;
        private readonly IReservaRepository _reservaRepository;

        public LugarService(ILugarRepository lugarRepository, IParqueRepository parqueRepository, IReservaRepository reservaRepository)
        {
            _lugarRepository = lugarRepository;
            _parqueRepository = parqueRepository;
            _reservaRepository = reservaRepository;
        }
        public async Task<IEnumerable<Lugar>> FindAvailableAsync(int freguesiaId, DateTime inicio, DateTime fim)
        {
            List<Lugar> todoslugares = new List<Lugar>();
            var parques = _parqueRepository.GetAllAsync().Result.Where(p => p.FreguesiaId == freguesiaId);
            foreach (Parque p in parques)
            {
                var ltemp = _lugarRepository.GetAllAsync().Result.Where(l => l.ParqueId == p.Id);
                todoslugares.AddRange(ltemp);
            }

            List<Lugar> ocupados = new List<Lugar>();
            var reservas = _reservaRepository.GetAllAsync().Result.Where(r =>
                r.Fim > inicio && r.Fim <= fim ||
                r.Inicio >= inicio && r.Inicio < fim ||
                r.Inicio >= inicio && r.Fim <= fim ||
                r.Inicio <= inicio && r.Fim >= fim);
            foreach (Reserva r in reservas)
            {
                var l = await _lugarRepository.GetByIdAsync(r.LugarId);
                ocupados.Add(l);
            }

            var disponiveis = todoslugares.Except(ocupados);

            return disponiveis;
        }
    }
}
