using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API_Parque_Publico.Models;

namespace API_Parque_Publico.Data
{
    public class API_Parque_PublicoContext : DbContext
    {
        public API_Parque_PublicoContext (DbContextOptions<API_Parque_PublicoContext> options)
            : base(options)
        {
        }

        public DbSet<API_Parque_Publico.Models.Cliente> Clientes { get; set; }

        public DbSet<API_Parque_Publico.Models.Freguesia> Freguesias { get; set; }

        public DbSet<API_Parque_Publico.Models.Lugar> Lugares { get; set; }

        public DbSet<API_Parque_Publico.Models.Parque> Parques { get; set; }

        public DbSet<API_Parque_Publico.Models.Reserva> Reservas { get; set; }
    }
}
