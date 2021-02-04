using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Models
{
    public class DebitoDireto : Credencial
    {
        public int Id { get; set; }
        public string Iban { get; set; }
        public string Nome { get; set; }
        public string Rua { get; set; }
        public string CodigoPostal { get; set; }
        public string Freguesia { get; set; }
        public DateTime DataSubscricao { get ; set; }
        public int MetodoId { get; set; }

        public MetodoPagamento Metodo { get; set; }
    }
}
