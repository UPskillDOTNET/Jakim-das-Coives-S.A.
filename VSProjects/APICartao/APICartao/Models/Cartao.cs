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
        public int Numero { get; set; }
        [Required]
        public string Nome { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DataValidade { get; set; }

        [Required]
        public int Cvc { get; set; }

        [Required]
        public double Custo { get; set; }
        
        [Required]
        public int NifDestinatario { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }

    }
}
