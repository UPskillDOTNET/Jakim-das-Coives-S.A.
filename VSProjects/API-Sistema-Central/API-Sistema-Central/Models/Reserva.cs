using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string NifUtilizador { get; set; }
        public int ParqueId { get; set; }
        [DataType(DataType.Currency)]
        [Range(typeof(double), "0", "10000")]
        public double Custo { get; set; }
        public int TransacaoId { get; set; }
        public int ReservaParqueId { get; set; }

        [ForeignKey("NifUtilizador")]
        public Utilizador Utilizador { get; set; }
        [ForeignKey("ParqueId")]
        public Parque Parque { get; set; }
        [ForeignKey("TransacaoId")]
        public Transacao Transacao { get; set; }
    }
}
