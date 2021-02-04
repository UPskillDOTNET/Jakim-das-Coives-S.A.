using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API_Sistema_Central.Models
{
    public class Utilizador : IdentityUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Range(typeof(int), "100000000", "999999999")]
        public string Nif { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Range(typeof(double), "0", "1000000")]
        public double Carteira { get; set; }
        [Required]
        public int CredencialId { get; set; }

        [ForeignKey("CredencialId")]
        public Credencial Credencial { get; set; }
    }
}
