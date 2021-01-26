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
        [Required]
        [ForeignKey("User")]
        [Range(typeof(int), "100000000", "999999999")]
        public int UserNIf { get; set; }
        [Required]
        [ForeignKey("Lugar")]
        public int LugarId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd:hh}", ApplyFormatInEditMode = true)]
        public DateTime Inicio { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd:hh}", ApplyFormatInEditMode = true)]
        public DateTime Fim { get; set; }

        public virtual User User { get; set; }
        public virtual Lugar Lugar { get; set; }
    }
}
