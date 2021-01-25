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

        public DbSet<PublicParkAPI.Models.User> User { get; set; }

        public DbSet<PublicParkAPI.Models.Lugar> Lugar { get; set; }

        public DbSet<PublicParkAPI.Models.Reserva> Reserva { get; set; }
    }
}
