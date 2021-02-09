using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_SubAluguer.Data;
using API_SubAluguer.Models;
using API_SubAluguer.Services;

namespace API_SubAluguer.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClientesController(IClienteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAllCartao()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("{nif}")]
        public async Task<ActionResult<Cliente>> GetCliente(int nif)
        {
            var cliente = await _service.GetByIdAsync(nif);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpPut("{nif}")]
        public async Task<IActionResult> PutCliente(int nif, Cliente cliente)
        {
            if (nif != cliente.Nif)
            {
                return BadRequest();
            }
            try
            {
                await _service.PutAsync(cliente);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            await _service.PostAsync(cliente);

            return CreatedAtAction("GetCliente", new { nif = cliente.Nif }, cliente);
        }

        [HttpDelete("{nif}")]
        public async Task<IActionResult> DeleteCliente(int nif)
        {
            var cartao = await _service.GetByIdAsync(nif);
            if (cartao == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(nif);

            return NoContent();
        }
    }
}
