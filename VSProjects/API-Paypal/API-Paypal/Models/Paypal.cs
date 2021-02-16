using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Paypal.Models
{
     public class Paypal
    {

        public int Id { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailDestinatario { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }

        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8)]
        public string Password { get; set; }
        public double Custo { get; set; }     

    }
  
}
