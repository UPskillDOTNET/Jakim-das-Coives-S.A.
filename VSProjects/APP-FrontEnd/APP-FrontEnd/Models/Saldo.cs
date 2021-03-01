using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APP_FrontEnd.Models
{
    public class Saldo
    {
        [Range(0, double.MaxValue)]
        public double Valor { get; set; } 
    }
}
