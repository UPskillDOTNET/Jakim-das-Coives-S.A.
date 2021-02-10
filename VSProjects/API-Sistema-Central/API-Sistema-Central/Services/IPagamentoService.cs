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

        public void Pay(PagamentoDTO payDTO);
        public void PayWithCarteira(string nif);
        public void PayWithCartao(CartaoDTO dTO);
        public void PayWithPayPal(PayPalDTO dTO);
        public void PayWithDebitoDireto(DebitoDiretoDTO dTO);
    }
}
