using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api_DebitoDireto.Data;
using Api_DebitoDireto.Models;

namespace Api_DebitoDireto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebitosDiretosController : ControllerBase
    {
        private readonly Api_DebitoDiretoContext _context;

        public DebitosDiretosController(Api_DebitoDiretoContext context)
        {
            _context = context;
        }

        // GET: api/DebitosDiretos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DebitoDireto>>> GetDebitoDireto()
        {
            return await _context.DebitoDireto.ToListAsync();
        }

        // GET: api/DebitosDiretos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DebitoDireto>> GetDebitoDireto(int id)
        {
            var debitoDireto = await _context.DebitoDireto.FindAsync(id);

            if (debitoDireto == null)
            {
                return NotFound();
            }

            return debitoDireto;
        }


        // POST: api/DebitosDiretos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DebitoDireto>> PostDebitoDireto(DebitoDireto debitoDireto)
        {
            _context.DebitoDireto.Add(debitoDireto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDebitoDireto", new { id = debitoDireto.id }, debitoDireto);
        }


        private bool DebitoDiretoExists(int id)
        {
            return _context.DebitoDireto.Any(e => e.id == id);
        }
    }
}
