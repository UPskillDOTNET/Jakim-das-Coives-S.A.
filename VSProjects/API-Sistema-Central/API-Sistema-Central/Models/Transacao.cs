using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Models
{
    public class Transacao
    {
        public int Id { get; set; }
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string NifPagador { get; set; }
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string NifRecipiente { get; set; }
        [DataType(DataType.Currency)]
        [Range(typeof(double), "0", "10000")]
        public double Valor { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DataHora { get; set; }
        public int MetodoId { get; set; }
        public Tipo Tipo { get; set; }

        [ForeignKey("NifPagador")]
        public Utilizador Pagador { get; set; }
        [ForeignKey("NifRecipiente")]
        public Utilizador Recipiente { get; set; }
        [ForeignKey("MetodoId")]
        public MetodoPagamento Metodo { get; set; }
    }
}
