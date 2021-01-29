using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Parque_Privado_3.Models
{
    public class Lugar
    {
        public int Id { get; set; }
        [Required]
        public int ParqueId { get; set; }
        [Required]
        public int Numero { get; set; }
        [Required]
        public string Fila { get; set; }
        [Required]
        public int Andar { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public double Preco { get; set; }


        [ForeignKey("ParqueId")]
        public Parque Parque { get; set; }
    }
}
