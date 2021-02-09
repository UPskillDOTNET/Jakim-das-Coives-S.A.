using API_Sistema_Central.DTOs;
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
        public Task<ActionResult> PayWithCartao(CartaoDTO dTO);
        public Task<ActionResult> PayWithPayPal(PayPalDTO dTO);
        public Task<ActionResult> PayWithDebitoDireto(DebitoDiretoDTO dTO);
    }
}
