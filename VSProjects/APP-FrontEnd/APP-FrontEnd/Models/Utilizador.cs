using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace APP_FrontEnd.Models
{
    public class Utilizador : IdentityUser
    {
        public int MetodoId { get; set; }
        public string Nome { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        [DataType(DataType.Currency)]
        [Range(typeof(double), "0", "1000000")]
        public double Carteira { get; set; }
        public int CredencialId { get; set; }
    }
}
