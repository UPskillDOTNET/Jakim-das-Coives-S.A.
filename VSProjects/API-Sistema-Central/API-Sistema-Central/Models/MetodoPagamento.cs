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
        public string Nome { get; set; }
        public string ApiUrl { get; set; }
    }
}
