using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.DTOs
{
    public class CartaoDTO
    {
        public string Numero { get; set; }
        public string Nome { get; set; }
        public string DataValidade { get; set; }
        public string Cvv { get; set; }
        [DataType(DataType.Currency)]
        public double Custo { get; set; }
        public int NifDestinatario { get; set; }
        public DateTime Data { get; set; }
    }
}
