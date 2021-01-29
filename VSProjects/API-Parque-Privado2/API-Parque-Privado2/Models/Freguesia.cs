using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Parque_Privado2.Models
{
    public class Freguesia
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
    }
}
