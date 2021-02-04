using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Sistema_Central.Data;
using API_Sistema_Central.Models;

namespace API_Sistema_Central.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebitosDiretosController : ControllerBase
    {
        private readonly SCContext _context;

        public DebitosDiretosController(SCContext context)
        {
            _context = context;
        }

        // GET: api/DebitosDiretos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DebitoDireto>>> GetDebitoDireto()
        {
            return await _context.DebitosDiretos.ToListAsync();
        }

        // GET: api/DebitosDiretos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DebitoDireto>> GetDebitoDireto(int id)
        {
            var debitoDireto = await _context.DebitosDiretos.FindAsync(id);

            if (debitoDireto == null)
            {
                return NotFound();
            }

            return debitoDireto;
        }

        // PUT: api/DebitosDiretos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDebitoDireto(int id, DebitoDireto debitoDireto)
        {
            if (id != debitoDireto.Id)
            {
                return BadRequest();
            }

            _context.Entry(debitoDireto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DebitoDiretoExists(id))
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

        // POST: api/DebitosDiretos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DebitoDireto>> PostDebitoDireto(DebitoDireto debitoDireto)
        {
            _context.DebitosDiretos.Add(debitoDireto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDebitoDireto", new { id = debitoDireto.Id }, debitoDireto);
        }

        // DELETE: api/DebitosDiretos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDebitoDireto(int id)
        {
            var debitoDireto = await _context.DebitosDiretos.FindAsync(id);
            if (debitoDireto == null)
            {
                return NotFound();
            }

            _context.DebitosDiretos.Remove(debitoDireto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DebitoDiretoExists(int id)
        {
            return _context.DebitosDiretos.Any(e => e.Id == id);
        }
    }
}
