using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APP_FrontEnd.Models
{
    public class TransacaoDTO
    {
        public string Tipo { get; set; }
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string NifDestinatario { get; set; }
        [DataType(DataType.Currency)]
        [Range(typeof(double), "0", "10000")]
        public double Valor { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DataHora { get; set; }
        public string Metodo { get; set; }
    }
}
