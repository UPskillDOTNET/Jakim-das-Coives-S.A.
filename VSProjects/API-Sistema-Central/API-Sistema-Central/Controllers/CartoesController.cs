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
    [Route("api/cartoes")]
    [ApiController]
    public class CartoesController : ControllerBase
    {
        private readonly ICartaoService _service;

        public CartoesController(ICartaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cartao>>> GetAllCartao()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cartao>> GetCartao(int id)
        {
            var cartao = await _service.GetByIdAsync(id);

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
            try
            {
                await _service.PutAsync(cartao);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Cartao>> PostCartao(Cartao cartao)
        {
            await _service.PostAsync(cartao);

            return CreatedAtAction("GetCartao", new { id = cartao.Id }, cartao);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartao(int id)
        {
            var cartao = await _service.GetByIdAsync(id);
            if (cartao == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
