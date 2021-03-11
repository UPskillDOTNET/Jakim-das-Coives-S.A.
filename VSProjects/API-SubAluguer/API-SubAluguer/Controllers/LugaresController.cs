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
    [Route("api/lugares")]
    [ApiController]
    public class LugaresController : ControllerBase
    {
        private readonly ILugarService _service;

        public LugaresController(ILugarService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lugar>>> GetAllLugar()
        {
            var lista = await _service.GetAllAsync();
            return lista.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Lugar>> GetLugarById(int id)
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

        [HttpGet("all/{nif}")]
        public async Task<ActionResult<IEnumerable<Lugar>>> GetLugarByNif(string nif)
        {
            try
            {
                var lista = await _service.GetByNifAsync(nif);
                return lista.ToList();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Lugar>> PostLugar(Lugar lugar)
        {
            try
            {
                Lugar l = await _service.PostAsync(lugar);
                return CreatedAtAction("GetLugarById", new { id = l.Id }, l);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("remover")]
        public async Task<IActionResult> RemoverUtilizador(NifDTO nif)
        {
            try
            {
                await _service.RemoverUtilizadorAsync(nif.Nif);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLugar(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("disponibilidade/{freguesiaId}/{inicio}/{fim}")]
        public IEnumerable<Lugar> FindAvailable(int freguesiaId, DateTime inicio, DateTime fim)
        {
            return _service.FindAvailable(freguesiaId, inicio, fim);
        }
    }
}
