using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Parque_Privado_3.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        [Required]
        public int NifCliente { get; set; }
        [Required]
        public int LugarId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Inicio { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Fim { get; set; }


        [ForeignKey("NifCliente")]
        public Cliente Cliente { get; set; }
        [ForeignKey("LugarId")]
        public Lugar Lugar { get; set; }
    }
}
