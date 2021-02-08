using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICartao.Data;
using APICartao.Models;
using APICartao.Repositories;
using APICartao.Services;

namespace APICartao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartoesController : ControllerBase
    {
        private readonly CartaoService _service;

        public CartoesController(CartaoService service)
        {
            _service = service;
        }

        // GET: api/Cartoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cartao>>> GetCartao()
        {
            return await _service.GetAllCartoes();
        }

        // GET: api/Cartoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cartao>> GetCartao(long id)
        {
            var cartao = await _service.GetCartaoById(id);

            if (cartao == null)
            {
                return NotFound();
            }

            return cartao;
        }

        [HttpGet("nr/{n}")]
        public async Task<ActionResult<IEnumerable<Cartao>>> GetCartoesByNumero(string n)
        {
            return await _service.GetCartoesByNumero(n);
        }

        // POST: api/Cartoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cartao>> PostCartao(Cartao cartao)
        {
            await _service.CreateNewCartao(cartao);

            return CreatedAtAction("GetCartao", new { id = cartao.Id }, cartao);
        }
    }
}
