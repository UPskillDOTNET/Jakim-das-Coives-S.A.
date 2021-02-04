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
        [Required]
        [Range(typeof(int), "100000000", "999999999")]
        public int NifUtilizador { get; set; }
        [Required]
        public int ParqueId { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Range(typeof(double), "0", "10000")]
        public double Custo { get; set; }
        [Required]
        public int TransacaoId { get; set; }
        [Required]
        public int ReservaParqueId { get; set; }

        [ForeignKey("NifUtilizador")]
        public Utilizador Utilizador { get; set; }
        [ForeignKey("ParqueId")]
        public Parque Parque { get; set; }
        [ForeignKey("TransacaoId")]
        public Transacao Transacao { get; set; }
    }
}
