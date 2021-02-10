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
    [Route("api/lugares")]
    [ApiController]
    public class LugaresController : ControllerBase
    {
        private readonly ILugarService _service;

        public LugaresController(ILugarService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lugar>>> GetAllLugar()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Lugar>> GetLugar(int id)
        {
            var lugar = await _service.GetByIdAsync(id);

            if (lugar == null)
            {
                return NotFound();
            }

            return lugar;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLugar(int id, Lugar lugar)
        {
            if (id != lugar.Id)
            {
                return BadRequest();
            }
            try
            {
                await _service.PutAsync(lugar);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Lugar>> PostLugar(Lugar lugar)
        {
            await _service.PostAsync(lugar);

            return CreatedAtAction("GetLugar", new { id = lugar.Id }, lugar);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLugar(int id)
        {
            var lugar = await _service.GetByIdAsync(id);
            if (lugar == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
