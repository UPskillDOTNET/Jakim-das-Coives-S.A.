using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Models
{
    public class DebitoDireto : Credencial
    {
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
        public DateTime DataSubscricao { get ; set; }
        [Required]
        public int MetodoId { get; set; }

        [ForeignKey("MetodoId")]
        public MetodoPagamento Metodo { get; set; }
    }
}
