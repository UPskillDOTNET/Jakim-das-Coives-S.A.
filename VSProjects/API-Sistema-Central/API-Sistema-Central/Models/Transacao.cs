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
        [Required]
        [Range(typeof(int), "100000000", "999999999")]
        public int NifPagador { get; set; }
        [Required]
        [Range(typeof(int), "100000000", "999999999")]
        public int NifRecipiente { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Range(typeof(double), "0", "10000")]
        public double Valor { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime dataHora;
        public DateTime DataHora { get { return dataHora; } set => dataHora = DateTime.Now; }
        [Required]
        public int MetodoId { get; set; }

        [ForeignKey("NifPagador")]
        public Utilizador Pagador { get; set; }
        [ForeignKey("NifRecipiente")]
        public Utilizador Recipiente { get; set; }
        [ForeignKey("MetodoId")]
        public MetodoPagamento Metodo { get; set; }
    }
}
