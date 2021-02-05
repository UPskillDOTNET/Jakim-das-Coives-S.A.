using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API_Sistema_Central.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace API_Sistema_Central.Data
{
    public class SCContext : IdentityDbContext
    {
        public SCContext (DbContextOptions<SCContext> options)
            : base(options)
        {
        }

        public DbSet<API_Sistema_Central.Models.Cartao> Cartoes { get; set; }

        public DbSet<API_Sistema_Central.Models.MetodoPagamento> MetodosPagamento { get; set; }

        public DbSet<API_Sistema_Central.Models.Parque> Parques { get; set; }

        public DbSet<API_Sistema_Central.Models.PayPal> PayPal { get; set; }

        public DbSet<API_Sistema_Central.Models.DebitoDireto> DebitosDiretos { get; set; }

        public DbSet<API_Sistema_Central.Models.Reserva> Reserva { get; set; }

        public DbSet<API_Sistema_Central.Models.Transacao> Transacao { get; set; }
    }
}
