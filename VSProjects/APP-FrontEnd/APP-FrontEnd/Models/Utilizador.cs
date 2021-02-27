using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace APP_FrontEnd.Models
{
    public class Utilizador : IdentityUser
    {
        public int MetodoId { get; set; }
        public string Nome { get; set; }
    }
}
