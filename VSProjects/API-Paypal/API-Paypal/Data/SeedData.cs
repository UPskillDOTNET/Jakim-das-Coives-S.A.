using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Paypal.Models;

namespace API_Paypal.Data
{
    public class SeedData
    {
          public static void Initialize(API_PaypalContext context)
          {
             // Look for any Cliente.
             if (context.Paypal.Any())
             {
                return;   // DB was already seeded
             }

            context.Paypal.AddRange(  //privados podem ter vários clientes
                new Paypal { Custo = 11, Password = "Jak&mCo&ves", Email = "jakimdascoives@upskill.pt", EmailDestinatario = "jakimdascoives@upskill.pt", Data = DateTime.Now},
                new Paypal { Custo = 11, Password = "Jak&mCo&ves", Email = "jakimdascoives@upskill.pt", EmailDestinatario = "jakimdascoives@upskill.pt", Data = DateTime.Now},
                new Paypal { Custo = 11, Password = "Jak&mCo&ves", Email = "jakimdascoives@upskill.pt", EmailDestinatario = "jakimdascoives@upskill.pt", Data = DateTime.Now},
                new Paypal { Custo = 11, Password = "Jak&mCo&ves", Email = "jakimdascoives@upskill.pt", EmailDestinatario = "jakimdascoives@upskill.pt", Data = DateTime.Now}
            );
            context.SaveChanges();
          }
    }
}
