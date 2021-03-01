using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP_FrontEnd.Models
{
    public class TokenUtilizadorDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
