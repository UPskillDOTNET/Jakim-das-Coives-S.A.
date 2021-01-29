using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API_Parque_Privado2.Models;

namespace API_Parque_Privado2.Data
{
    public class API_Parque_Privado2Context : DbContext
    {
        public API_Parque_Privado2Context (DbContextOptions<API_Parque_Privado2Context> options)
            : base(options)
        {
        }

        public DbSet<API_Parque_Privado2.Models.Cliente> Clientes { get; set; }

        public DbSet<API_Parque_Privado2.Models.Freguesia> Freguesias { get; set; }

        public DbSet<API_Parque_Privado2.Models.Lugar> Lugares { get; set; }

        public DbSet<API_Parque_Privado2.Models.Parque> Parques { get; set; }

        public DbSet<API_Parque_Privado2.Models.Reserva> Reservas { get; set; }
    }
}
