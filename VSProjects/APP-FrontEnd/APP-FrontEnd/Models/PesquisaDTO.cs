using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APP_FrontEnd.Models
{
    public class PesquisaDTO
    {
        [Display(Name = "Qual a Freguesia?")]
        public string FreguesiaNome { get; set; }

        [Display(Name = "Data de Início")]
        public DateTime Inicio { get; set; }

        [Display(Name = "Data de Fim")]
        public DateTime Fim { get; set; }
    }
}
