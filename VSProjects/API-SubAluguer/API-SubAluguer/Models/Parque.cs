using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Models
{
    public class Parque
    {
        public int Id { get; set; }
        public string Rua { get; set; }
        public int FreguesiaId { get; set; }

        [ForeignKey("FreguesiaId")]
        public Freguesia Freguesia { get; set; }
    }
}
