using API_SubAluguer.Data;
using API_SubAluguer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Repository
{
    public class DisponibilidadeRepository : RepositoryBase<Disponibilidade>, IDisponibilidadeRepository
    {
        public DisponibilidadeRepository(API_SubAluguerContext context) : base(context)
        {
        }
        public async Task<ActionResult<IEnumerable<Disponibilidade>>> GetAllIncludeAsync()
        {
            return await _context.Set<Disponibilidade>().Include(r => r.Lugar).ToListAsync();
        }
    }
}
