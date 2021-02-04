using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API_Paypal.Models;

namespace API_Paypal.Data
{
    public class API_PaypalContext : DbContext
    {
        public API_PaypalContext (DbContextOptions<API_PaypalContext> options)
            : base(options)
        {
        }

        public DbSet<API_Paypal.Models.Paypal> Paypal { get; set; }
    }
}
