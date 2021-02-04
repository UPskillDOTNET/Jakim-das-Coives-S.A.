using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Api_DebitoDireto.Models;

namespace Api_DebitoDireto.Data
{
    public class Api_DebitoDiretoContext : DbContext
    {
        public Api_DebitoDiretoContext (DbContextOptions<Api_DebitoDiretoContext> options)
            : base(options)
        {
        }

        public DbSet<Api_DebitoDireto.Models.DebitoDireto> DebitoDireto { get; set; }
    }
}
