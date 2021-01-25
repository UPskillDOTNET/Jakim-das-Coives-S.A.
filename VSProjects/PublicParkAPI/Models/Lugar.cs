using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicParkAPI.Models
{
    public class Lugar
    {
        public int LugarId { get; set; }
        public int Numero { get; set; }
        public string Fila { get; set; }
        public int Andar { get; set; }
        public string Rua { get; set; }
        public string Freguesia { get; set; }
        public double Preco { get; set; }

    }
}
