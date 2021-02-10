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
    public class TransacoesController : ControllerBase
    {
        private readonly ITransacaoService _service;

        public TransacoesController(ITransacaoService service)
        {
            _service = service;
        }

        [HttpGet("{nif}")]
        public async Task<ActionResult<IEnumerable<Transacao>>> GetTransacaoByNif(string nif)
        {
            return await _service.GetByNifAsync(nif);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transacao>> GetTransacaoById(int id)
        {
            var transacao = await _service.GetByIdAsync(id);

            if (transacao == null)
            {
                return NotFound();
            }

            return transacao;
        }
    }
}
