using API_Sistema_Central.DTOs;
using API_Sistema_Central.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Sistema_Central.Services
{
    public interface IPagamentoService
    {

        public Task Pay(PagamentoDTO payDTO);
        public Task PayWithCarteira(Utilizador payingUser, Utilizador receivingUser, double valor);
        public Task PayWithCartao(CartaoDTO dTO);
        public Task PayWithPayPal(PayPalDTO dTO);
        public Task PayWithDebitoDireto(DebitoDiretoDTO dTO);
    }
}
