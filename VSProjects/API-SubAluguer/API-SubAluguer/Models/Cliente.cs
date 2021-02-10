using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_SubAluguer.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Range(typeof(int), "100000000", "999999999")]
        public int Nif { get; set; }

        public string Nome { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
