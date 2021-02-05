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
    public class MetodosPagamentoController : ControllerBase
    {
        private readonly SCContext _context;

        public MetodosPagamentoController(SCContext context)
        {
            _context = context;
        }

        // GET: api/MetodoPagamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MetodoPagamento>>> GetMetodoPagamento()
        {
            return await _context.MetodosPagamento.ToListAsync();
        }

        // GET: api/MetodoPagamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MetodoPagamento>> GetMetodoPagamento(int id)
        {
            var metodoPagamento = await _context.MetodosPagamento.FindAsync(id);

            if (metodoPagamento == null)
            {
                return NotFound();
            }

            return metodoPagamento;
        }

        // PUT: api/MetodoPagamentos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMetodoPagamento(int id, MetodoPagamento metodoPagamento)
        {
            if (id != metodoPagamento.Id)
            {
                return BadRequest();
            }

            _context.Entry(metodoPagamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MetodoPagamentoExists(id))
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

        // POST: api/MetodoPagamentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MetodoPagamento>> PostMetodoPagamento(MetodoPagamento metodoPagamento)
        {
            _context.MetodosPagamento.Add(metodoPagamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMetodoPagamento", new { id = metodoPagamento.Id }, metodoPagamento);
        }

        // DELETE: api/MetodoPagamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMetodoPagamento(int id)
        {
            var metodoPagamento = await _context.MetodosPagamento.FindAsync(id);
            if (metodoPagamento == null)
            {
                return NotFound();
            }

            _context.MetodosPagamento.Remove(metodoPagamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MetodoPagamentoExists(int id)
        {
            return _context.MetodosPagamento.Any(e => e.Id == id);
        }
    }
}
