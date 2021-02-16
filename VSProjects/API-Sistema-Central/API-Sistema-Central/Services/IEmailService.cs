using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;

namespace API_Sistema_Central.Services
{
    public interface IEmailService
    {
        public void EnviarEmailReserva(QRCodeDTO qr);
        public void EnviarEmailSubAluguer(QRCodeDTO qr, int reservaOriginalId);
        public void EnviarEmailCancelamento(string nome, int id, string email);
    }
}
