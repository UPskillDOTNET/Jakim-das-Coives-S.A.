using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Parque_Publico.Data;
using API_Parque_Publico.Models;

namespace API_Parque_Publico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LugarsController : ControllerBase
    {
        private readonly API_Parque_PublicoContext _context;

        public LugarsController(API_Parque_PublicoContext context)
        {
            _context = context;
        }

        // GET: api/Lugars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lugar>>> GetLugar()
        {
            return await _context.Lugar.ToListAsync();
        }

        // GET: api/Lugars/5
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

        // PUT: api/Lugars/5
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

        // POST: api/Lugars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lugar>> PostLugar(Lugar lugar)
        {
            _context.Lugar.Add(lugar);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLugar", new { id = lugar.LugarId }, lugar);
        }

        // DELETE: api/Lugars/5
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
