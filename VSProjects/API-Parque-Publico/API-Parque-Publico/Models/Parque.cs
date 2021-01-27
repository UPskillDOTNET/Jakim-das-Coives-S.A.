using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Parque_Publico.Models
{
    public class Parque
    {
        public int Id { get; set; }
        [Required]
        public string Rua { get; set; }
        [Required]
        public int FreguesiaId { get; set; }


        [ForeignKey("FreguesiaId")]
        public Freguesia Freguesia { get; set; }
    }
}
