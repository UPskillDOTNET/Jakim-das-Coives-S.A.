using Api_DebitoDireto.Data;
using Api_DebitoDireto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_DebitoDireto.Repositories
{
    public class DebitoDiretoRepository : IDebitoDiretoRepository
    {
        private readonly Api_DebitoDiretoContext _context;

        public DebitoDiretoRepository(Api_DebitoDiretoContext context)
        {
            _context = context;
        }
        public async Task<ActionResult<IEnumerable<DebitoDireto>>> ReturnAllDebitoDireto()
        {
            return await _context.DebitoDireto.ToListAsync();
        }
        public async Task<DebitoDireto> PostDebitoDireto(DebitoDireto debitoDireto)
        {
            EntityEntry<DebitoDireto> entry;
            entry = await _context.DebitoDireto.AddAsync(debitoDireto);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }
    }
}
