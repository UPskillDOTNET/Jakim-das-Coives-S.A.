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
        [Required]
        [Key]
        public int ParqueId { get; set; }
        [Required]
        public string Rua { get; set; }
        [Required]
        [ForeignKey("Freguesia")]
        public int FreguesiaId { get; set; }
        public virtual Freguesia Freguesia { get; set; }


    }
}
