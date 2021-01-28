using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Parque_Privado.Models;

namespace API_Parque_Privado.Data
{
    public class SeedData
    {
        public static void Initialize(API_Parque_PrivadoContext context)
        {
            // Look for any Cliente.
            if (context.Clientes.Any())
            {
                return;   // DB was already seeded
            }

            context.Clientes.AddRange(
                new Cliente { Nif = 111222333 , Nome = "Sistema Central", Email = "jakimdascoives@upskill.pt" }
            );
            context.SaveChanges();

            context.Freguesias.AddRange(
                new Freguesia { Nome = "Porto" },
                new Freguesia { Nome = "Lisboa" },
                new Freguesia { Nome = "Braga" },
                new Freguesia { Nome = "Portimão" },
                new Freguesia { Nome = "Évora" }
            );
            context.SaveChanges();

            context.Parques.AddRange(
                new Parque { Rua = "D. Joao", FreguesiaId = 1 },
                new Parque { Rua = "D. Jose", FreguesiaId = 1 },
                new Parque { Rua = "Da. Maria", FreguesiaId = 1 },
                new Parque { Rua = "D. Daniel", FreguesiaId = 1 },
                new Parque { Rua = "D. Raphael", FreguesiaId = 1 }
            );
            context.SaveChanges();

            context.Lugares.AddRange(
                new Lugar { Numero = 1, Fila = "A", Andar = -2, ParqueId = 1, Preco = 5.99 },
                new Lugar { Numero = 2, Fila = "B", Andar = -1, ParqueId = 2, Preco = 5.99 },
                new Lugar { Numero = 3, Fila = "C", Andar = 0, ParqueId = 3, Preco = 5.99 },
                new Lugar { Numero = 4, Fila = "D", Andar = 1, ParqueId = 4, Preco = 5.99 },
                new Lugar { Numero = 5, Fila = "E", Andar = 2, ParqueId = 5, Preco = 5.99 }
            );
            context.SaveChanges();

            context.Reservas.AddRange(
                new Reserva { NifCliente = 111222333, LugarId = 1, Inicio = DateTime.Parse("2021-01-30 11:00:00"), Fim = DateTime.Parse("2021-01-30 12:00:00") },
                new Reserva { NifCliente = 111222333, LugarId = 2, Inicio = DateTime.Parse("2021-01-29 21:00:00"), Fim = DateTime.Parse("2021-01-31 10:00:00") },
                new Reserva { NifCliente = 111222333, LugarId = 3, Inicio = DateTime.Parse("2021-01-30 10:00:00"), Fim = DateTime.Parse("2021-01-30 13:00:00") },
                new Reserva { NifCliente = 111222333, LugarId = 4, Inicio = DateTime.Parse("2021-01-30 11:00:00"), Fim = DateTime.Parse("2021-01-30 13:00:00") }
            );
            context.SaveChanges();
        }
    }
}