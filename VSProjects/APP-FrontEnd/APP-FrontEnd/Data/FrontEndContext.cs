using APP_FrontEnd.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_FrontEnd.Data
{
    public class FrontEndContext : IdentityDbContext<Utilizador>
    {
        public FrontEndContext(DbContextOptions<FrontEndContext> options)
            : base(options)
        {
        }
    }
}
