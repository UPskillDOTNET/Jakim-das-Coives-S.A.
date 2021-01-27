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

            context.Clientes.AddRange(
                new Cliente { Nif = 111222333 , Nome = "Jakim", Email = "jakim@upskill.pt" }
            );
            
            context.Freguesias.AddRange(
                new Freguesia { Nome = "Porto" }
            );
            
            context.Parques.AddRange(
                new Parque { Rua = "D. Joao" , FreguesiaId = 1 }
            );
            
            context.Lugares.AddRange(
                new Lugar { Numero = 1, Fila = "A" , Andar = 0, ParqueId = 1 , Preco = 5.99 }
            );

            context.Reservas.AddRange(
                new Reserva { NifCliente = 111222333, LugarId = 1, Inicio = DateTime.Parse("2021-01-30 11:00:00") , Fim = DateTime.Parse("2021-01-30 12:00:00") }
            );

            context.SaveChanges();
        }
    }
}