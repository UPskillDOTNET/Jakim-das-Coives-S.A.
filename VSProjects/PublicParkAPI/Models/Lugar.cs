using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PublicParkAPI.Models
{
    public class Lugar
    {
        public int LugarId { get; set; }
        [Required]
        public int Numero { get; set; }
        public string Fila { get; set; }
        public int? Andar { get; set; }
        public string Rua { get; set; }
        [Required]
        public string Freguesia { get; set; }
        public string Parque { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public double Preco { get; set; }
    }
}
