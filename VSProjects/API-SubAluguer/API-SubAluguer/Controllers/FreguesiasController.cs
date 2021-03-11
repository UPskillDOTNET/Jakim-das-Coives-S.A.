using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_SubAluguer.Models;
using API_SubAluguer.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<Freguesia>>> GetAllLugar()
        {
            var lista = await _service.GetAllAsync();
            return lista.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Freguesia>> GetLugarById(int id)
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
