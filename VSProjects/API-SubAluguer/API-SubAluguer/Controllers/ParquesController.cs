using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_SubAluguer.Models;
using API_SubAluguer.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_SubAluguer.Controllers
{
    [Route("api/parques")]
    [ApiController]
    public class ParquesController : ControllerBase
    {
        private readonly IParqueService _service;

        public ParquesController(IParqueService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parque>>> GetAllParques()
        {
            var lista = await _service.GetAllAsync();
            return lista.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Parque>> GetParqueById(int id)
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
    }
}
