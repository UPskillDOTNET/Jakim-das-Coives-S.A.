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
            try
            {
                return await _service.FindAvailableAsync(freguesiaNome, inicio, fim);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("all/{nif}")]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservaByNif(string nif)
        {
            try
            {
                return await _service.GetByNifAsync(nif);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReservaById(int id)
        {
            try
            {
                return await _service.GetByIdAsync(id);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva(ReservaDTO reservaDTO)
        {
            try
            {
                Reserva reserva = await _service.PostAsync(reservaDTO);
                return CreatedAtAction("GetReservaById", new { id = reserva.Id }, reserva);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
