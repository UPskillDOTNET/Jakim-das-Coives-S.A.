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

        public DbSet<API_SubAluguer.Models.Cliente> Clientes { get; set; }

        public DbSet<API_SubAluguer.Models.Freguesia> Freguesias { get; set; }

        public DbSet<API_SubAluguer.Models.Lugar> Lugares { get; set; }

        public DbSet<API_SubAluguer.Models.Parque> Parques { get; set; }

        public DbSet<API_SubAluguer.Models.Reserva> Reservas { get; set; }
    }
}
