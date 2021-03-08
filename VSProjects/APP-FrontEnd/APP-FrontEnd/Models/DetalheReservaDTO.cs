using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APP_FrontEnd.Models
{
    public class DetalheReservaDTO
    {
        public int ReservaId { get; set; }
        [DataType(DataType.Currency)]
        public double Custo { get; set; }
        public int ReservaParqueId { get; set; }
        public string NomeFreguesia { get; set; }
        public string NomeParque { get; set; }
        public int NumeroLugar { get; set; }
        public string Fila { get; set; }
        public int Andar { get; set; }
        public bool IsSubAlugado { get; set; }
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string NifProprietario { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Inicio { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Fim { get; set; }
    }
}
