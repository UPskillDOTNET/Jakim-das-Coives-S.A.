using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.Data;
using API_Sistema_Central.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Sistema_Central.Repositories
{
    public class CartaoRepository : ICartaoRepository
    {
        private readonly SCContext _context;

        public CartaoRepository(SCContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Cartao>>> GetAllCartao()
        {
            return await _context.Cartoes.ToListAsync();
        }
    }
}
