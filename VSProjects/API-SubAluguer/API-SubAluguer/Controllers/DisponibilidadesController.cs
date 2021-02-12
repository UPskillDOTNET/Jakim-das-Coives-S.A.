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
    [Route("api/disponibilidades")]
    [ApiController]
    public class DisponibilidadesController : ControllerBase
    {
        private readonly IDisponibilidadeService _service;

        public DisponibilidadesController(IDisponibilidadeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Disponibilidade>>> GetAllDisponibilidade()
        {

            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Disponibilidade>> GetDisponibilidade(int id)
        {
            var disponibilidade = await _service.GetByIdAsync(id);

            if (disponibilidade == null)
            {
                return NotFound();
            }

            return disponibilidade;
        }

        [HttpPost]
        public async Task<ActionResult<Disponibilidade>> PostDisponibilidade(Disponibilidade disponibilidade)
        {
            await _service.PostAsync(disponibilidade);

            return CreatedAtAction("GetDisponibilidade", new { id = disponibilidade.Id }, disponibilidade);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisponibilidade(int id)
        {
            var disponibilidade = await _service.GetByIdAsync(id);
            if (disponibilidade == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
