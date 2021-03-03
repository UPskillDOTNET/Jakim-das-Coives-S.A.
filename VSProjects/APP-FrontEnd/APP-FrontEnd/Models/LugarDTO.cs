using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APP_FrontEnd.Models
{
    public class LugarDTO
    {
        public int Id { get; set; }
        public int ParqueId { get; set; }
        public string NomeParque { get; set; }
        public int Numero { get; set; }
        public string Fila { get; set; }
        public int Andar { get; set; }
        [DataType(DataType.Currency)]
        public double Preco { get; set; }
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string NifProprietario { get; set; }
        public int ReservaSistemaCentralId { get; set; }
        public int ParqueIdSC { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Inicio { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Fim { get; set; }
    }
}
