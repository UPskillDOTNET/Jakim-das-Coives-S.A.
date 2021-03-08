using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.DTOs
{
    public class PayPalDTO
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EmailDestinatario { get; set; }
        public DateTime Data { get; set; }
        public string Password { get; set; }
        [DataType(DataType.Currency)]
        public double Custo { get; set; }
    }
}
