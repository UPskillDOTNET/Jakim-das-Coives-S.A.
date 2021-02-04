using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Models
{
    public class Cartao : Credencial
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Numero do cartão de crédito inválido")]
        public string Numero { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        [RegularExpression(@"^\d{2}/\d{2}$", ErrorMessage = "Data de Validade do cartão de crédito inválido")]
        public string DataValidade { get; set; }
        [Required]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV do cartão de crédito inválido")]
        public string Cvv { get; set; }
        [Required]
        public int MetodoId { get; set; }

        [ForeignKey("MetodoId")]
        public MetodoPagamento Metodo { get; set; }
    }
}
