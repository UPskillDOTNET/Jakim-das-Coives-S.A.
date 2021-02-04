using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.DTOs
{
    public class RegistarUtilizadorDTO
    {
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string Nif { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
