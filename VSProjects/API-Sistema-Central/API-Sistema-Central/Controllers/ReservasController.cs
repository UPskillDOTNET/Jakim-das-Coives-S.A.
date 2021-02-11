using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Data;
using API_Sistema_Central.Models;
using API_Sistema_Central.Services;

namespace API_Sistema_Central.Controllers
{
    [Route("api/reservas")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _service;

        public ReservasController(IReservaService service)
        {
            _service = service;
        }

        [HttpGet("disponibilidade/{freguesiaNome}/{inicio}/{fim}")]
        public async Task<ActionResult<IEnumerable<LugarDTO>>> FindAvailableAsync(string freguesiaNome, DateTime inicio, DateTime fim)
        {
            return await _service.FindAvailableAsync(freguesiaNome, inicio, fim);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservaByNif(string nif)
        {
            var reservas = await _service.GetByNifAsync(nif);

            if (reservas == null)
            {
                return NotFound();
            }

            return reservas;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReservaById(int id)
        {
            var reserva = await _service.GetByIdAsync(id);

            if (reserva == null)
            {
                return NotFound();
            }

            return reserva;
        }

        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva(ReservaDTO reservaDTO)
        {
            Reserva reserva = await _service.PostAsync(reservaDTO);

            return CreatedAtAction("GetReservaById", new { id = reserva.Id }, reserva);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _service.GetByIdAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
