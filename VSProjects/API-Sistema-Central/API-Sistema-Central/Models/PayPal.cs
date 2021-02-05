using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Models
{
    public class PayPal : Credencial
    {
        public override int Id { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public override int MetodoId { get; set; }

        [ForeignKey("MetodoId")]
        public MetodoPagamento Metodo { get; set; }
    }
}
