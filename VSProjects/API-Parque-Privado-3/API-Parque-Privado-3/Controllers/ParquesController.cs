using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Parque_Privado_3.Data;
using API_Parque_Privado_3.Models;

namespace API_Parque_Privado_3.Controllers
{
    [Route("api/parques")]
    [ApiController]
    public class ParquesController : ControllerBase
    {
        private readonly API_Parque_Privado_3Context _context;

        public ParquesController(API_Parque_Privado_3Context context)
        {
            _context = context;
        }

        // GET: api/Parques
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parque>>> GetParque()
        {
            return await _context.Parques.ToListAsync();
        }

        // GET: api/Parques/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Parque>> GetParque(int id)
        {
            var parque = await _context.Parques.FindAsync(id);

            if (parque == null)
            {
                return NotFound();
            }

            return parque;
        }

        // PUT: api/Parques/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParque(int id, Parque parque)
        {
            if (id != parque.Id)
            {
                return BadRequest();
            }

            _context.Entry(parque).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParqueExists(id))
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

        // POST: api/Parques
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Parque>> PostParque(Parque parque)
        {
            _context.Parques.Add(parque);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParque", new { id = parque.Id }, parque);
        }

        // DELETE: api/Parques/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParque(int id)
        {
            var parque = await _context.Parques.FindAsync(id);
            if (parque == null)
            {
                return NotFound();
            }

            _context.Parques.Remove(parque);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParqueExists(int id)
        {
            return _context.Parques.Any(e => e.Id == id);
        }
    }
}
