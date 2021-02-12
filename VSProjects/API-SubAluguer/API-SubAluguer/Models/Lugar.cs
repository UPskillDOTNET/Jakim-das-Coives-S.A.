using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Models
{
    public class Lugar
    {
        public int Id { get; set; }
        public int ParqueId { get; set; }
        public int Numero { get; set; }
        public string Fila { get; set; }
        public int Andar { get; set; }
        [DataType(DataType.Currency)]
        public double Preco { get; set; }
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string NifProprietario { get; set; }


        [ForeignKey("ParqueId")]
        public Parque Parque { get; set; }
    }
}
