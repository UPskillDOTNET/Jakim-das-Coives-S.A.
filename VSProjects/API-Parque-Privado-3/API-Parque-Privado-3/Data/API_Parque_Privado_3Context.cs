using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API_Parque_Privado_3.Models;

namespace API_Parque_Privado_3.Data
{
    public class API_Parque_Privado_3Context : DbContext
    {
        public API_Parque_Privado_3Context (DbContextOptions<API_Parque_Privado_3Context> options)
            : base(options)
        {
        }

        public DbSet<API_Parque_Privado_3.Models.Cliente> Clientes { get; set; }

        public DbSet<API_Parque_Privado_3.Models.Freguesia> Freguesias { get; set; }

        public DbSet<API_Parque_Privado_3.Models.Lugar> Lugares { get; set; }

        public DbSet<API_Parque_Privado_3.Models.Parque> Parques { get; set; }

        public DbSet<API_Parque_Privado_3.Models.Reserva> Reservas { get; set; }
    }
}
