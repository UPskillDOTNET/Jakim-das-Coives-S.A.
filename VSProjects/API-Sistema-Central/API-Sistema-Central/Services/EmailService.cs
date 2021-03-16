using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace API_Sistema_Central.Services
{
    public interface IEmailService
    {
        public Task EnviarEmailReservaAsync(QRCodeDTO qr);
        public Task EnviarEmailSubAluguerAsync(QRCodeDTO qr, int reservaId);
        public Task EnviarEmailCancelamentoAsync(string nome, int id, string email);
    }

    public class EmailService : IEmailService
    {
        public async Task EnviarEmailReservaAsync(QRCodeDTO qr)
        {
            try
            {
                string subject = "Confirmação de reserva";
                string conteudoqr = "Reserva nº " + qr.ReservaParqueId + ", Parque: " + qr.NomeParque;
                string qrcode = "<img src='https://api.qrserver.com/v1/create-qr-code/?size=300x300&data=" + conteudoqr + "'/>";
                string body = "<h2>Exmo(a) Sr.(a) " + qr.NomeUtilizador + "</h2>" +
                    "<h2>A sua reserva número " + qr.ReservaParqueId + " está pronta!</h2>" +
                    "<table><tr><td><p><b>Lugar: " + qr.NumeroLugar + "</b></p>" +
                    "<p><b>Fila: " + qr.Fila + "</b></p>" +
                    "<p><b>Andar: " + qr.Andar + "</b></p><br>" +
                    "<p>Freguesia: " + qr.NomeFreguesia + "</p>" +
                    "<p>Parque: " + qr.NomeParque + "</p><br>" +
                    "<p>Data e Hora de início: " + qr.Inicio + "</p>" +
                    "<p>Data e Hora de fim: " + qr.Fim + "</p></td><td style='width: 30px'></td><td>" + qrcode + "</td></tr></table>";

                await EnviarEmailAsync(subject, qr.Email, body);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task EnviarEmailSubAluguerAsync(QRCodeDTO qr, int reservaId)
        {
            try
            {
                string subject = "Confirmação de reserva";
                string conteudoqr = "Reserva nº " + reservaId + ", Sub-Reserva nº " + qr.ReservaParqueId + ", Parque: " + qr.NomeParque;
                string qrcode = "<img src='https://api.qrserver.com/v1/create-qr-code/?size=300x300&data=" + conteudoqr + "'/>";
                string body = "<h2>Exmo(a) Sr.(a) " + qr.NomeUtilizador + "</h2>" +
                    "<h2>A sua reserva número " + reservaId + ", sub-reserva número " + qr.ReservaParqueId + " está pronta!</h2>" +
                    "<table><tr><td><p><b>Lugar: " + qr.NumeroLugar + "</b></p>" +
                    "<p><b>Fila: " + qr.Fila + "</b></p>" +
                    "<p><b>Andar: " + qr.Andar + "</b></p><br>" +
                    "<p>Freguesia: " + qr.NomeFreguesia + "</p>" +
                    "<p>Parque: " + qr.NomeParque + "</p><br>" +
                    "<p>Data e Hora de início: " + qr.Inicio + "</p>" +
                    "<p>Data e Hora de fim: " + qr.Fim + "</p></td><td style='width: 30px'></td><td>" + qrcode + "</td></tr></table>";

                await EnviarEmailAsync(subject, qr.Email, body);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task EnviarEmailCancelamentoAsync(string nome, int reservaParqueId, string email)
        {
            try
            {
                string subject = "Cancelamento de reserva";
                string body = "<h2>Exmo(a) Sr.(a) " + nome + "</h2>" +
                "<h2>A sua reserva número " + reservaParqueId + " foi cancelada com sucesso!</h2>";

                await EnviarEmailAsync(subject, email, body);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static async Task EnviarEmailAsync(string subject, string email, string body)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("sistemacentraljakim@gmail.com");
            var to = new EmailAddress(email);
            var plainTextContent = "";
            var htmlContent = body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
