using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Parque_Privado2.Data;
using API_Parque_Privado2.Models;

namespace API_Parque_Privado2.Controllers
{
    [Route("api/freguesias")]
    [ApiController]
    public class FreguesiasController : ControllerBase
    {
        private readonly API_Parque_Privado2Context _context;

        public FreguesiasController(API_Parque_Privado2Context context)
        {
            _context = context;
        }

        // GET: api/Freguesias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Freguesia>>> GetFreguesia()
        {
            return await _context.Freguesias.ToListAsync();
        }

        // GET: api/Freguesias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Freguesia>> GetFreguesia(int id)
        {
            var freguesia = await _context.Freguesias.FindAsync(id);

            if (freguesia == null)
            {
                return NotFound();
            }

            return freguesia;
        }

        // PUT: api/Freguesias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFreguesia(int id, Freguesia freguesia)
        {
            if (id != freguesia.Id)
            {
                return BadRequest();
            }

            _context.Entry(freguesia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FreguesiaExists(id))
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

        // POST: api/Freguesias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Freguesia>> PostFreguesia(Freguesia freguesia)
        {
            _context.Freguesias.Add(freguesia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFreguesia", new { id = freguesia.Id }, freguesia);
        }

        // DELETE: api/Freguesias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFreguesia(int id)
        {
            var freguesia = await _context.Freguesias.FindAsync(id);
            if (freguesia == null)
            {
                return NotFound();
            }

            _context.Freguesias.Remove(freguesia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FreguesiaExists(int id)
        {
            return _context.Freguesias.Any(e => e.Id == id);
        }
    }
}
