using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Parque_Publico.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Range(typeof(int), "100000000", "999999999")]
        public int Nif { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Email { get; set; }

        //[InverseProperty("Cliente")]
        //public ICollection<Reserva> Reservas { get; set; }
    }
}
