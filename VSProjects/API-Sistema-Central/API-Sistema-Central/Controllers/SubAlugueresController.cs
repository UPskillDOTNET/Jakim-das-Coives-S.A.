using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Controllers
{
    //[Authorize]
    [Route("api/subalugueres")]
    [ApiController]
    public class SubAlugueresController : ControllerBase
    {
        private readonly ISubAluguerService _service;

        public SubAlugueresController(ISubAluguerService service)
        {
            _service = service;
        }

        [HttpGet("all/{nif}")]
        public async Task<ActionResult<IEnumerable<SubAluguerDTO>>> GetSubAluguerByNif(string nif)
        {
            try
            {
                return await _service.GetByNifAsync(nif);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubAluguerDTO>> GetSubAluguerById(int id)
        {
            try
            {
                return await _service.GetByIdAsync(id);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<SubAluguerDTO>> PostSubAlugarLugarAsync(SubAluguerDTO subAluguerDTO)
        {
            try
            {
                SubAluguerDTO result = await _service.PostSubAluguerAsync(subAluguerDTO);
                return CreatedAtAction("GetSubAluguerById", new { id = result.Id }, result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            try
            {
                await _service.DeleteSubAluguerAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
