using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Sistema_Central.Repositories
{
    public interface ICartaoRepository
    {
        public Task<ActionResult<IEnumerable<Cartao>>> GetAllCartao();
    }
}
