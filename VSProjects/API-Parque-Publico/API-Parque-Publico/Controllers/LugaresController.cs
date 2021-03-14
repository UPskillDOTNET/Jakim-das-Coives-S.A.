using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Parque_Publico.Data;
using API_Parque_Publico.Models;
using API_Parque_Publico.Services;

namespace API_Parque_Publico.Controllers
{
    [Route("api/lugares")]
    [ApiController]
    public class LugaresController : ControllerBase
    {
        private readonly API_Parque_PublicoContext _context;
        private readonly ILugarService _lugarService;

        public LugaresController(API_Parque_PublicoContext context, ILugarService lugarService)
        {
            _context = context;
            _lugarService = lugarService;
        }

        // GET: api/Lugares
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lugar>>> GetLugar()
        {
            return await _context.Lugares.ToListAsync();
        }

        // GET: api/Lugares/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lugar>> GetLugar(int id)
        {
            var lugar = await _context.Lugares.FindAsync(id);

            if (lugar == null)
            {
                return NotFound();
            }

            return lugar;
        }

        [HttpGet("disponibilidade/{freguesiaId}/{inicio}/{fim}")]
        public async Task<ActionResult<IEnumerable<Lugar>>> FindAvailableAsync(int freguesiaId, DateTime inicio, DateTime fim)
        {
            try
            {
                var disponiveis = await _lugarService.FindAvailableAsync(freguesiaId, inicio, fim);

                return disponiveis.ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Lugares/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLugar(int id, Lugar lugar)
        {
            if (id != lugar.Id)
            {
                return BadRequest();
            }

            _context.Entry(lugar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LugarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Lugares
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lugar>> PostLugar(Lugar lugar)
        {
            _context.Lugares.Add(lugar);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLugar", new { id = lugar.Id }, lugar);
        }

        // DELETE: api/Lugares/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLugar(int id)
        {
            var lugar = await _context.Lugares.FindAsync(id);
            if (lugar == null)
            {
                return NotFound();
            }

            _context.Lugares.Remove(lugar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LugarExists(int id)
        {
            return _context.Lugares.Any(e => e.Id == id);
        }
    }
}
