using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_SubAluguer.Data;
using API_SubAluguer.Models;
using API_SubAluguer.Services;

namespace API_SubAluguer.Controllers
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetAllReserva()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id)
        {
            var reserva = await _service.GetByIdAsync(id);

            if (reserva == null)
            {
                return NotFound();
            }

            return reserva;
        }

        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva(Reserva reserva)
        {
            await _service.PostAsync(reserva);

            return CreatedAtAction("GetReserva", new { id = reserva.Id }, reserva);
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
