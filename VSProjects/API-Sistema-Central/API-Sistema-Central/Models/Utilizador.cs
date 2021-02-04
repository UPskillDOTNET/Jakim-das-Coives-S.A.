using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Models
{
    public class Utilizador
    {
        public int Nif { get; set; }
        public string Nome { get; set; }
        public double Carteira { get; set; }
        public int CredencialId { get; set; }

        public Credencial Credencial { get; set; }
    }
}
