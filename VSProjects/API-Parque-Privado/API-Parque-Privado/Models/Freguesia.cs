using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Parque_Privado.Models
{
    public class Freguesia
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
    }
}
