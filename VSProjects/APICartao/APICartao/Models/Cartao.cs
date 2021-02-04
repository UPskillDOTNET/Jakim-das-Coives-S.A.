using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APICartao.Models
{
    public class Cartao
    {
        public int Id { get; set; }

        [Required]
        [MinLength(16)]
        [MaxLength(16)]
        public string Numero { get; set; }
        [Required]
        public string Nome { get; set; }

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2])[/]\d{2}$", ErrorMessage = "Data de Validade do cartão de crédito inválido")]
        public string DataValidade { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string Cvv { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double Custo { get; set; }
        
        [Required]
        public int NifDestinatario { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }

    }
}
