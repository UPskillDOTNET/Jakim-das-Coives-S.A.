using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_DebitoDireto.Models;
using Api_DebitoDireto.Services;
using Microsoft.AspNetCore.Mvc;


namespace Api_DebitoDireto.Repositories
{
    public interface IDebitoDiretoRepository
    {
        public Task<ActionResult<IEnumerable<DebitoDireto>>> ReturnAllDebitoDireto();
        public Task<DebitoDireto> PostDebitoDireto(DebitoDireto debitoDireto);
    }
}
