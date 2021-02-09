using API_Sistema_Central.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Services
{
    public class PagamentoService : IPagamentoService
    {
        Task<ActionResult> IPagamentoService.PayWithCartao(CartaoDTO dTO)
        {
            throw new NotImplementedException();
        }

        Task<ActionResult> IPagamentoService.PayWithCarteira(string nif)
        {
            throw new NotImplementedException();
        }

        Task<ActionResult> IPagamentoService.PayWithDebitoDireto(DebitoDiretoDTO dTO)
        {
            throw new NotImplementedException();
        }

        Task<ActionResult> IPagamentoService.PayWithPayPal(PayPalDTO dTO)
        {
            throw new NotImplementedException();
        }
    }
}
