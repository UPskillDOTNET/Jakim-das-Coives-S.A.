using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PublicParkAPI.Models
{
    public class Reserva
    {

        public int ReservaId { get; set; }

        [ForeignKey("Nif")]
        public User User { get; set; }

        [ForeignKey("LugarId")]
        public Lugar Lugar { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Inicio { get; set; }
        public int Duracao { get; set; }

    }
}
