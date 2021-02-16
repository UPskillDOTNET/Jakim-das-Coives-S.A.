using APICartao.Data;
using APICartao.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICartao.Repositories
{
    public class CartaoRepository : RepositoryBase<Cartao>, ICartaoRepository
    {
        public CartaoRepository(APICartaoContext theContext) : base(theContext)
        {
        }

        public async Task<ActionResult<IEnumerable<Cartao>>> GetCartoesByNumeroAsync(string numero)
        {
            var query = _context.Cartao.Where(a => a.Numero == numero);
            return await query.ToListAsync();
        }
    }
}
