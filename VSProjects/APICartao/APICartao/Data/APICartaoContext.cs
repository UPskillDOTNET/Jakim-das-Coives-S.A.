using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APICartao.Models;

namespace APICartao.Data
{
    public class APICartaoContext : DbContext
    {
        public APICartaoContext (DbContextOptions<APICartaoContext> options)
            : base(options)
        {
        }

        public DbSet<APICartao.Models.Cartao> Cartao { get; set; }
    }
}
