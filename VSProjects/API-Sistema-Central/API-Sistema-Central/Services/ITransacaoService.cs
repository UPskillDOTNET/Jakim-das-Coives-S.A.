using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Services
{
    public interface ITransacaoService
    {
        public Task<Transacao> GetByIdAsync(int id);
        public Task<ActionResult<IEnumerable<TransacaoDTO>>> GetByNifAsync(string nif);
    }
}
