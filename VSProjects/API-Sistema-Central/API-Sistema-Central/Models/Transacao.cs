using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Models
{
    public class Transacao
    {
        public int Id { get; set; }
        public int NifPagador { get; set; }
        public int NifRecipiente { get; set; }
        public double Valor { get; set; }
        public DateTime dataHora;
        public DateTime DataHora { get { return dataHora; } set => dataHora = DateTime.Now; }
        public int metodoId { get; set; }

        public Utilizador Pagador { get; set; }
        public Utilizador Recipiente { get; set; }
        public MetodoPagamento Metodo { get; set; }
    }
}
