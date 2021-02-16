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
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[P][T][5][0]\d{21}$", ErrorMessage = "IBAN inválido")]
        public string Iban { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Rua { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "Código postal inválido")]
        public string CodigoPostal { get; set; }
        [Required]
        public string Freguesia { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
        [Required]
        [Range(typeof(int), "100000000", "999999999")]
        public int NifDestinatario { get; set; }
        [Required]
        public double Custo { get; set; }


    }
}
