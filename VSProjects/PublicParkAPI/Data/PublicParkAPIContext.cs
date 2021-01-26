using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PublicParkAPI.Models;

namespace PublicParkAPI.Data
{
    public class PublicParkAPIContext : DbContext
    {
        public PublicParkAPIContext (DbContextOptions<PublicParkAPIContext> options)
            : base(options)
        {
        }

        public DbSet<PublicParkAPI.Models.User> Users { get; set; }

        public DbSet<PublicParkAPI.Models.Lugar> Lugares { get; set; }

        public DbSet<PublicParkAPI.Models.Reserva> Reservas { get; set; }
    }
}
