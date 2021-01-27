using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Parque_Publico.Models
{
    public class Reserva
    {
        [Required]
        [Key]
        public int ReservaId { get; set; }
        [Required]
        [ForeignKey("Cliente")]
        public int NifCliente { get; set; }
        [Required]
        [ForeignKey("Lugar")]
        public int LugarId { get; set; }
        [Required]
        public DateTime Inicio { get; set; }
        [Required]
        public DateTime Fim { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Lugar Lugar { get; set; }

    }
}
