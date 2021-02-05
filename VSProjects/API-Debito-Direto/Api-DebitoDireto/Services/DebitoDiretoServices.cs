using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api_DebitoDireto.Models;
using Api_DebitoDireto.Controllers;
using Api_DebitoDireto.Repositories;


namespace Api_DebitoDireto.Services
{
    public class DebitoDiretoServices : IDebitoDiretoService
    {
        private readonly IDebitoDiretoRepository _repository;

        public DebitoDiretoServices(IDebitoDiretoRepository repository)
        {
            _repository = repository;
        }
        public async Task<ActionResult<IEnumerable<DebitoDireto>>> GetAllDebitoDireto()
        {
            return await _repository.ReturnAllDebitoDireto();
        }

        public async Task<ActionResult<DebitoDireto>> PostDebitoDireto(DebitoDireto debitoDireto)
        {
            return await _repository.PostDebitoDireto(debitoDireto);
        }
    }
}
