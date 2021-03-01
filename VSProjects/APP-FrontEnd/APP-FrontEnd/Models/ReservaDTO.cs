using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APP_FrontEnd.Models
{
    public class ReservaDTO
    {
        //Para usar na reserva da API-Sistema-Central
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string NifComprador { get; set; }
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string NifVendedor { get; set; }
        public int ParqueIdSC { get; set; }
        public int MetodoId { get; set; }

        //Apenas para Sub-Alugueres
        public int ReservaSistemaCentralId { get; set; }

        //Para usar na reserva da API-Parque
        public int LugarId { get; set; }
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Inicio { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Fim { get; set; }
        
    }
}
