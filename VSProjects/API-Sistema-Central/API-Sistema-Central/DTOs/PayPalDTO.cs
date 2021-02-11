using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.DTOs
{
    public class PayPalDTO
    {
        public string Email { get; set; }
        public string EmailDestinatario { get; set; }
        public DateTime Data { get; set; }
        public string Password { get; set; }
        public double Custo { get; set; }
    }
}
