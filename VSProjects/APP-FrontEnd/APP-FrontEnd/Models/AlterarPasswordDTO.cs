using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APP_FrontEnd.Models
{
    public class AlterarPasswordDTO
    {
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string Nif { get; set; }

        [DataType(DataType.Password)]
        public string PasswordActual { get; set; }
        [DataType(DataType.Password)]
        public string PasswordNova { get; set; }
    }
}
