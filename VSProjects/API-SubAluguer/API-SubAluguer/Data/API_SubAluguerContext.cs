using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API_SubAluguer.Models;

namespace API_SubAluguer.Data
{
    public class API_SubAluguerContext : DbContext
    {
        public API_SubAluguerContext (DbContextOptions<API_SubAluguerContext> options)
            : base(options)
        {
        }

        public DbSet<API_SubAluguer.Models.Cliente> Cliente { get; set; }

        public DbSet<API_SubAluguer.Models.Freguesia> Freguesia { get; set; }

        public DbSet<API_SubAluguer.Models.Lugar> Lugar { get; set; }

        public DbSet<API_SubAluguer.Models.Parque> Parque { get; set; }

        public DbSet<API_SubAluguer.Models.Reserva> Reserva { get; set; }

        public DbSet<API_SubAluguer.Models.Disponibilidade> Disponibilidade { get; set; }
    }
}
