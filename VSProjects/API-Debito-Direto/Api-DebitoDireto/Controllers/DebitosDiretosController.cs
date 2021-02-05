using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api_DebitoDireto.Data;
using Api_DebitoDireto.Models;
using Api_DebitoDireto.Services;

namespace Api_DebitoDireto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebitosDiretosController : ControllerBase
    {
        private readonly IDebitoDiretoService _service;

        public DebitosDiretosController(IDebitoDiretoService service)
        {
            _service = service;
        }

        // GET: api/DebitosDiretos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DebitoDireto>>> GetAllDebitoDireto()
        {
            return await _service.GetAllDebitoDireto();
        }


        // POST: api/DebitosDiretos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DebitoDireto>> PostDebitoDireto(DebitoDireto debitoDireto)
        {
            await _service.PostDebitoDireto(debitoDireto);

            return CreatedAtAction("GetDebitoDireto", new { id = debitoDireto.Id }, debitoDireto);
        }



    }
}
