using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Models
{
    public class Cartao : Credencial
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public string Nome { get; set; }
        public DateTime DataValidade { get; set; }
        public int Cvc { get; set; }
        public int MetodoId { get; set; }

        public MetodoPagamento Metodo { get; set; }
    }
}
