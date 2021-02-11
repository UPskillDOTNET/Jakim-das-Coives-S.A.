using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.DTOs
{
    public class ParqueDTO
    {
        public int Id { get; set; }
        public string Rua { get; set; }
        public int FreguesiaId { get; set; }
    }
}
