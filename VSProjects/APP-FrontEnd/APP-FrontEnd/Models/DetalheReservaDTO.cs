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

        [Display(Name = "Preço")]
        public double Custo { get; set; }

        [Display(Name = "Nº da Reserva")]
        public int ReservaParqueId { get; set; }

        [Display(Name = "Freguesia")]
        public string NomeFreguesia { get; set; }

        [Display(Name = "Parque")]
        public string NomeParque { get; set; }

        [Display(Name = "Nº de Lugar")]
        public int NumeroLugar { get; set; }
        public string Fila { get; set; }
        public int Andar { get; set; }

        [Display(Name = "Encontra-se Sub-Alugado?")]
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
