using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public int NifUtilizador { get; set; }
        public int ParqueId { get; set; }
        public double Custo { get; set; }
        public int TransacaoId { get; set; }
        public int ReservaParqueId { get; set; }


        public Utilizador Utilizador { get; set; }
        public Parque Parque { get; set; }
        public Transacao Transacao { get; set; }
    }
}
