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
    [Route("api/parques")]
    [ApiController]
    public class ParquesController : ControllerBase
    {
        private readonly IParqueService _service;

        public ParquesController(IParqueService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parque>>> GetAllParque()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Parque>> GetParque(int id)
        {
            var lugar = await _service.GetByIdAsync(id);

            if (lugar == null)
            {
                return NotFound();
            }

            return lugar;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutParque(int id, Parque parque)
        {
            if (id != parque.Id)
            {
                return BadRequest();
            }
            try
            {
                await _service.PutAsync(parque);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Parque>> PostParque(Parque parque)
        {
            await _service.PostAsync(parque);

            return CreatedAtAction("GetParque", new { id = parque.Id }, parque);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParque(int id)
        {
            var parque = await _service.GetByIdAsync(id);
            if (parque == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
