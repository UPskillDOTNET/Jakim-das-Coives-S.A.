﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.DTOs
{
    public class TokenUtilizadorDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
