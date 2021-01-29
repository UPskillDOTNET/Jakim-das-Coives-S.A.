using API_Parque_Privado2.Data;
using API_Parque_Privado2.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace UnitTest
{
    class API_Parque_PrivadoContextMocker
    {
        private static API_Parque_Privado2Context dbContext;
        public static API_Parque_Privado2Context GetPrivParkContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<API_Parque_Privado2Context>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
            dbContext = new API_Parque_Privado2Context(options);
            Seed();
            return dbContext;
        }

        private static void Seed()
        {
            /*
                Seed do Teste será igual ao seed da base de dados
            */


            dbContext.Clientes.AddRange(  //privados podem ter vários clientes
               new Cliente { Nif = 111222333, Nome = "Sistema Central", Email = "jakimdascoives@upskill.pt" },
               new Cliente { Nif = 123456789, Nome = "Joao", Email = "joao@upskill.pt" },
               new Cliente { Nif = 987654321, Nome = "Jose", Email = "jose@upskill.pt" },
               new Cliente { Nif = 112233445, Nome = "Raphael", Email = "raphael@upskill.pt" },
               new Cliente { Nif = 998877665, Nome = "Ines", Email = "ines@upskill.pt" },
               new Cliente { Nif = 554466887, Nome = "Daniel", Email = "daniel@upskill.pt" }
           );
            dbContext.SaveChanges();

            dbContext.Freguesias.AddRange( //apenas uma freguesia
                new Freguesia { Nome = "Currale de Moinas" }
            );
            dbContext.SaveChanges();

            dbContext.Parques.AddRange(
                new Parque { Rua = "Lugar das Moinas", FreguesiaId = 1 }
            );
            dbContext.SaveChanges();

            dbContext.Lugares.AddRange(
                new Lugar { Numero = 1, Fila = "AA", Andar = 1, ParqueId = 1, Preco = 5 },
                new Lugar { Numero = 2, Fila = "BB", Andar = 2, ParqueId = 1, Preco = 6 },
                new Lugar { Numero = 3, Fila = "CC", Andar = 3, ParqueId = 1, Preco = 7 },
                new Lugar { Numero = 4, Fila = "DD", Andar = 4, ParqueId = 1, Preco = 8 },
                new Lugar { Numero = 5, Fila = "EE", Andar = 5, ParqueId = 1, Preco = 9 }
            );
            dbContext.SaveChanges();

            dbContext.Reservas.AddRange(
                new Reserva { NifCliente = 123456789, LugarId = 1, Inicio = DateTime.Parse("2021-01-30 11:00:00"), Fim = DateTime.Parse("2021-01-30 12:00:00") },
                new Reserva { NifCliente = 987654321, LugarId = 2, Inicio = DateTime.Parse("2021-01-29 21:00:00"), Fim = DateTime.Parse("2021-01-31 10:00:00") },
                new Reserva { NifCliente = 112233445, LugarId = 3, Inicio = DateTime.Parse("2021-01-30 10:00:00"), Fim = DateTime.Parse("2021-01-30 13:00:00") },
                new Reserva { NifCliente = 998877665, LugarId = 4, Inicio = DateTime.Parse("2021-01-30 11:00:00"), Fim = DateTime.Parse("2021-01-30 13:00:00") },
                new Reserva { NifCliente = 554466887, LugarId = 4, Inicio = DateTime.Parse("2021-01-30 11:00:00"), Fim = DateTime.Parse("2021-01-30 13:00:00") }

            );
            dbContext.SaveChanges();
        }
    }
}
