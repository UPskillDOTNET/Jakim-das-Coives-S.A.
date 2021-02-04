using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Models
{
    public class PayPal : Credencial
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int MetodoId { get; set; }

        public MetodoPagamento Metodo { get; set; }
    }
}
