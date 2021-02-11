using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace API_Sistema_Central.Services
{
    public class EmailService : IEmailService
    {
        public void EnviarEmail()
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("sistemacentraljakim@gmail.com");
            msg.To.Add("sistemacentraljakim@gmail.com");
            msg.Subject = "Confirmação de reserva";
            msg.Body = "Test Content";

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("sistemacentraljakim@gmail.com", "123Pa$$word");
            smtp.EnableSsl = true;
            smtp.Send(msg);
        }
    }
}
