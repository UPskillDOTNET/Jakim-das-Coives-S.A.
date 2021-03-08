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
        public double Custo { get; set; }

        [Display(Name = "Nº da Reserva")]
        public int ReservaParqueId { get; set; }
        public string NomeFreguesia { get; set; }

        [Display(Name = "Parque")]
        public string NomeParque { get; set; }
        public int NumeroLugar { get; set; }
        public string Fila { get; set; }
        public int Andar { get; set; }
        public bool IsSubAlugado { get; set; }
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string NifProprietario { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Data de Início")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Inicio { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Data de Fim")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Fim { get; set; }
    }
}
