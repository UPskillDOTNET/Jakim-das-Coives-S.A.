using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.DTOs
{
    public class PagamentoDTO
    {
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string NifUtilizador { get; set; }
        [RegularExpression(@"^\d{9}$", ErrorMessage = "NIF inválido")]
        public string NifVendedor { get; set; }
        public int MetodoId { get; set; }
        public double Preco { get; set; }
    }
}
