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
    [Route("api/freguesias")]
    [ApiController]
    public class FreguesiasController : ControllerBase
    {
        private readonly IFreguesiaService _service;

        public FreguesiasController(IFreguesiaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Freguesia>>> GetAllFreguesia()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Freguesia>> GetFreguesia(int id)
        {
            var freguesia = await _service.GetByIdAsync(id);

            if (freguesia == null)
            {
                return NotFound();
            }

            return freguesia;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFreguesia(int id, Freguesia freguesia)
        {
            if (id != freguesia.Id)
            {
                return BadRequest();
            }
            try
            {
                await _service.PutAsync(freguesia);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Freguesia>> PostFreguesia(Freguesia freguesia)
        {
            await _service.PostAsync(freguesia);

            return CreatedAtAction("GetFreguesia", new { id = freguesia.Id }, freguesia);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFreguesia(int id)
        {
            var freguesia = await _service.GetByIdAsync(id);
            if (freguesia == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
