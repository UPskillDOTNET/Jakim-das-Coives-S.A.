using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Services
{
    public interface IReservaService
    {
        public Task<ActionResult<IEnumerable<LugarDTO>>> FindAvailableAsync(string freguesiaNome, DateTime inicio, DateTime fim);
        public Task<ActionResult<IEnumerable<DetalheReservaDTO>>> GetByNifAsync(string nif);
        public Task<DetalheReservaDTO> GetByIdAsync(int id);
        public Task<Reserva> PostAsync(ReservaDTO reservaDTO);
        public Task DeleteAsync(int id);
    }
}
