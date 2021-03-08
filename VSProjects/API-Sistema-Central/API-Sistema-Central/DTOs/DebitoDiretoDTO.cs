using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.DTOs
{
    public class DebitoDiretoDTO
    {
        public string Iban { get; set; }
        public string Nome { get; set; }
        public string Rua { get; set; }
        public string CodigoPostal { get; set; }
        public string Freguesia { get; set; }
        public DateTime Data { get; set; }
        public int NifDestinatario { get; set; }
        [DataType(DataType.Currency)]
        public double Custo { get; set; }
    }
}
