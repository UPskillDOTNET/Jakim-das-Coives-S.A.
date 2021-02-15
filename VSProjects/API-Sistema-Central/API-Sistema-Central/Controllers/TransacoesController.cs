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
using Microsoft.AspNetCore.Authorization;

namespace API_Sistema_Central.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransacoesController : ControllerBase
    {
        private readonly ITransacaoService _service;

        public TransacoesController(ITransacaoService service)
        {
            _service = service;
        }

        [HttpGet("all/{nif}")]
        public async Task<ActionResult<IEnumerable<Transacao>>> GetTransacaoByNif(string nif)
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
        public async Task<ActionResult<Transacao>> GetTransacaoById(int id)
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
