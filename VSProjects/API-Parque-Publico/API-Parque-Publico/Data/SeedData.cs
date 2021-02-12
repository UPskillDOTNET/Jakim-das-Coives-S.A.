using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Parque_Publico.Models;

namespace API_Parque_Publico.Data
{
    public class SeedData
    {
        public static void Initialize(API_Parque_PublicoContext context)
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

            context.Lugares.Add(new Lugar { Numero = 12, Fila = "A", Andar = -2, ParqueId = 1, Preco = 5.99 });
            context.SaveChanges();
            context.Lugares.Add(new Lugar { Numero = 27, Fila = "B", Andar = -1, ParqueId = 1, Preco = 5.99 });
            context.SaveChanges();
            context.Lugares.Add(new Lugar { Numero = 4, Fila = "Lado Par da Rua", Andar = 0, ParqueId = 2, Preco = 5.99 });
            context.SaveChanges();
            context.Lugares.Add(new Lugar { Numero = 7, Fila = "Lado Ímpar da Rua", Andar = 0, ParqueId = 2, Preco = 5.99 });
            context.SaveChanges();
            context.Lugares.Add(new Lugar { Numero = 57, Fila = "E", Andar = 2, ParqueId = 3, Preco = 5.99 });
            context.SaveChanges();
            context.Lugares.Add(new Lugar { Numero = 12, Fila = "A", Andar = -2, ParqueId = 3, Preco = 5.99 });
            context.SaveChanges();
            context.Lugares.Add(new Lugar { Numero = 2, Fila = "B", Andar = -1, ParqueId = 4, Preco = 5.99 });
            context.SaveChanges();
            context.Lugares.Add(new Lugar { Numero = 36, Fila = "C", Andar = -2, ParqueId = 4, Preco = 5.99 });
            context.SaveChanges();
            context.Lugares.Add(new Lugar { Numero = 8, Fila = "Lado Par da Rua", Andar = 0, ParqueId = 5, Preco = 5.99 });
            context.SaveChanges();
            context.Lugares.Add(new Lugar { Numero = 5, Fila = "Lado Ímpar da Rua", Andar = 0, ParqueId = 5, Preco = 5.99 });
            context.SaveChanges();
            context.Lugares.Add(new Lugar { Numero = 18, Fila = "D", Andar = 0, ParqueId = 6, Preco = 5.99 });
            context.SaveChanges();
            context.Lugares.Add(new Lugar { Numero = 33, Fila = "E", Andar = 2, ParqueId = 6, Preco = 5.99 });
            context.SaveChanges();
        }
    }
}