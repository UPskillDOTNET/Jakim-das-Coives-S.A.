using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_SubAluguer.Models;
using API_SubAluguer.Controllers;

namespace API_SubAluguer.Data
{
    public class SeedData
    {
        public static void Initialize(API_SubAluguerContext context)
        {
            // Look for any Cliente.
            if (context.Clientes.Any())
            {
                return;   // DB was already seeded
            }

            context.Clientes.Add(new Cliente { Nif = 999999999, Nome = "Sistema Central", Email = "sistemacentraljakim@gmail.com" });
            context.SaveChanges();

            context.Freguesias.Add(new Freguesia { Nome = "Porto" });
            context.SaveChanges();
            context.Freguesias.Add(new Freguesia { Nome = "Lisboa" });
            context.SaveChanges();

            context.Parques.Add(new Parque { Rua = "Rua do parque privado 1", FreguesiaId = 1 });
            context.SaveChanges();
            context.Parques.Add(new Parque { Rua = "Rua do parque privado 2", FreguesiaId = 2 });
            context.SaveChanges();
            context.Parques.Add(new Parque { Rua = "Rua do parque privado 3", FreguesiaId = 1 });
            context.SaveChanges();
            context.Parques.Add(new Parque { Rua = "Parque Subterrâneo da Rua Porto 1", FreguesiaId = 1 });
            context.SaveChanges();
            context.Parques.Add(new Parque { Rua = "Rua Porto 2", FreguesiaId = 1 });
            context.SaveChanges();
            context.Parques.Add(new Parque { Rua = "Parque Coberto da Rua Porto 3", FreguesiaId = 1 });
            context.SaveChanges();
            context.Parques.Add(new Parque { Rua = "Parque Subterrâneo da Rua Lisboa 1", FreguesiaId = 2 });
            context.SaveChanges();
            context.Parques.Add(new Parque { Rua = "Rua Lisboa 2", FreguesiaId = 2 });
            context.SaveChanges();
            context.Parques.Add(new Parque { Rua = "Parque Coberto da Rua Lisboa 3", FreguesiaId = 2 });
            context.SaveChanges();
        }
    }
}
