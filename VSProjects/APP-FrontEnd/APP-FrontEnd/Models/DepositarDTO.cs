using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APP_FrontEnd.Models
{
    public class DepositarDTO
    {
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string Nif { get; set; }
        [Range(1, 10000, ErrorMessage = "Depósito mínimo de 1€ e máximo de 10.000€")]
        public double Valor { get; set; }
    }
}
