
using Api_DebitoDireto.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_DebitoDireto.Services
{
    public interface IDebitoDiretoService
    {
        public Task<ActionResult<IEnumerable<DebitoDireto>>> GetAllDebitoDireto();
        public Task<DebitoDireto>PostDebitoDireto(DebitoDireto debitoDireto);
    }

}
