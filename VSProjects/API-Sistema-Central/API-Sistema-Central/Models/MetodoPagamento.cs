using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Models
{
    public class MetodoPagamento
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string ApiUrl { get; set; }
    }
}
