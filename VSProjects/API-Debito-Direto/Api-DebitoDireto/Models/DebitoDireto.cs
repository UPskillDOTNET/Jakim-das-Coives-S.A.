using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Api_DebitoDireto.Models
{
    public class DebitoDireto
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string iban { get; set; }
        [Required]
        public string nome { get; set; }
        [Required]
        public string rua { get; set; }
        [Required]
        public string codigoPostal { get; set; }
        [Required]
        public string freguesia { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime data { get; set; }
        [Required]
        public int nifDestinatario { get; set; }
        [Required]
        public double custo { get; set; }


    }
}
