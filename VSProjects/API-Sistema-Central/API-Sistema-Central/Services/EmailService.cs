using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using API_Sistema_Central.DTOs;

namespace API_Sistema_Central.Services
{
    public class EmailService : IEmailService
    {
        public void EnviarEmail(QRCodeDTO qr)
        {
            string conteudoqr = "Reserva nº " + qr.IdReserva + ", Parque: " + qr.NomeParque;
            string qrcode = "<img src='https://api.qrserver.com/v1/create-qr-code/?size=300x300&data=" + conteudoqr + "'/>";
            string body = "<h2>Exmo(a) Sr.(a) " + qr.NomeUtilizador + "</h2>" +
                "<h2>A sua reserva número " + qr.IdReserva + " está pronta!</h2>" +
                "<table><tr><td><p><b>Lugar: " + qr.NumeroLugar + "</b></p>"+
                "<p><b>Fila: " + qr.Fila + "</b></p>" +
                "<p><b>Andar: " + qr.Andar + "</b></p><br>" +
                "<p>Freguesia: " + qr.NomeFreguesia + "</p>" +
                "<p>Parque: " + qr.NomeParque + "</p><br>" +
                "<p>Data e Hora de início: " + qr.Inicio + "</p>" +
                "<p>Data e Hora de fim: " + qr.Fim + "</p></td><td style='width: 30px'></td><td>" + qrcode + "</td></tr></table>";

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("sistemacentraljakim@gmail.com");
            msg.To.Add(qr.Email);
            msg.Subject = "Confirmação de reserva";
            msg.Body = body;
            msg.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("sistemacentraljakim@gmail.com", "123Pa$$word");
            smtp.EnableSsl = true;
            smtp.Send(msg);
        }

        public void EnviarEmailCancelamento(string nome, int id, string email)
        {
            string body = "<h2>Exmo(a) Sr.(a) " + nome + "</h2>" +
                "<h2>A sua reserva número " + id + " foi cancelada com sucesso!</h2>";

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("sistemacentraljakim@gmail.com");
            msg.To.Add(email);
            msg.Subject = "Cancelamento de reserva";
            msg.Body = body;
            msg.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("sistemacentraljakim@gmail.com", "123Pa$$word");
            smtp.EnableSsl = true;
            smtp.Send(msg);
        }
    }
}
