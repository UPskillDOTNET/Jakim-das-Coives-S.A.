using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Services
{
    public interface IPagamentoService
    {
        public Task<ActionResult> PayWithCarteira(string nif);
        public Task<ActionResult> PayWithCard();
        public Task<ActionResult> PayWithPayPal();
        public Task<ActionResult> PayWithDebitoDireto();
    }
}
