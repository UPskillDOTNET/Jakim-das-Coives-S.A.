using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API_Parque_Privado.Models;

namespace API_Parque_Privado.Data
{
    public class API_Parque_PrivadoContext : DbContext
    {
        public API_Parque_PrivadoContext (DbContextOptions<API_Parque_PrivadoContext> options)
            : base(options)
        {
        }

        public DbSet<API_Parque_Privado.Models.Cliente> Clientes { get; set; }

        public DbSet<API_Parque_Privado.Models.Freguesia> Freguesias { get; set; }

        public DbSet<API_Parque_Privado.Models.Lugar> Lugares { get; set; }

        public DbSet<API_Parque_Privado.Models.Parque> Parques { get; set; }

        public DbSet<API_Parque_Privado.Models.Reserva> Reservas { get; set; }
    }
}
