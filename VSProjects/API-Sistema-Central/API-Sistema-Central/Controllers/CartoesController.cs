using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Sistema_Central.Data;
using API_Sistema_Central.Models;
using API_Sistema_Central.Services;

namespace API_Sistema_Central.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartoesController : ControllerBase
    {
        private readonly SCContext _context;
        private readonly ICartaoService _service;

        public CartoesController(SCContext context, ICartaoService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cartao>>> GetCartao()
        {
            return await _service.GetAllCartao();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cartao>> GetCartao(int id)
        {
            var cartao = await _context.Cartoes.FindAsync(id);

            if (cartao == null)
            {
                return NotFound();
            }

            return cartao;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartao(int id, Cartao cartao)
        {
            if (id != cartao.Id)
            {
                return BadRequest();
            }

            _context.Entry(cartao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartaoExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Cartao>> PostCartao(Cartao cartao)
        {
            _context.Cartoes.Add(cartao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCartao", new { id = cartao.Id }, cartao);
        }

        // DELETE: api/Cartoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartao(int id)
        {
            var cartao = await _context.Cartoes.FindAsync(id);
            if (cartao == null)
            {
                return NotFound();
            }

            _context.Cartoes.Remove(cartao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartaoExists(int id)
        {
            return _context.Cartoes.Any(e => e.Id == id);
        }
    }
}
