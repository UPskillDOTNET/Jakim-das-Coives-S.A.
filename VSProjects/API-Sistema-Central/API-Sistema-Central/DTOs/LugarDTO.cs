using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.DTOs
{
    public class LugarDTO
    {
        public int Id { get; set; }
        public int ParqueId { get; set; }
        public int Numero { get; set; }
        public string Fila { get; set; }
        public int Andar { get; set; }
        [DataType(DataType.Currency)]
        public double Preco { get; set; }
        public string ApiUrl { get; set; }
    }
}
