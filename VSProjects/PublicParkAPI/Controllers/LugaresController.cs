using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicParkAPI.Data;
using PublicParkAPI.Models;

namespace PublicParkAPI.Controllers
{
    [Route("api/lugares")]
    [ApiController]
    public class LugaresController : ControllerBase
    {
        private readonly PublicParkAPIContext _context;

        public LugaresController(PublicParkAPIContext context)
        {
            _context = context;
        }

        // GET: api/Lugares
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lugar>>> GetLugar()
        {
            return await _context.Lugar.ToListAsync();
        }

        // GET: api/Lugares/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lugar>> GetLugar(int id)
        {
            var lugar = await _context.Lugar.FindAsync(id);

            if (lugar == null)
            {
                return NotFound();
            }

            return lugar;
        }

        // GET: api/lugares/available
        [HttpGet("available/{time}")]
        public async Task<ActionResult<IEnumerable<Lugar>>> GetAvailableLugaresAtTime(DateTime _time) //retornar lugares disponiveis a uma certa hora
        {
            throw new NotImplementedException();

            var result = new List<Lugar>();

        }

        // PUT: api/Lugares/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLugar(int id, Lugar lugar)
        {
            if (id != lugar.LugarId)
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
            _context.Lugar.Add(lugar);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLugar", new { id = lugar.LugarId }, lugar);
        }

        // DELETE: api/Lugares/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLugar(int id)
        {
            var lugar = await _context.Lugar.FindAsync(id);
            if (lugar == null)
            {
                return NotFound();
            }

            _context.Lugar.Remove(lugar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LugarExists(int id)
        {
            return _context.Lugar.Any(e => e.LugarId == id);
        }
    }
}
