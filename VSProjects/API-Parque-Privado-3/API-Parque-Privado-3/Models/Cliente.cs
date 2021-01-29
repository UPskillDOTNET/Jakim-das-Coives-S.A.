using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Parque_Privado_3.Models
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
    }
}
