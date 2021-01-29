using API_Parque_Privado.Data;
using API_Parque_Privado.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace UnitTest
{
    class API_Parque_PrivadoContextMocker
    {
        private static API_Parque_PrivadoContext dbContext;
        public static API_Parque_PrivadoContext GetPrivParkContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<API_Parque_PrivadoContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
            dbContext = new API_Parque_PrivadoContext(options);
            Seed();
            return dbContext;
        }

        private static void Seed()
        {
            /*
                Seed do Teste será igual ao seed da base de dados
            */


            dbContext.Clientes.AddRange(  //privados podem ter vários clientes
                new Cliente { Nif = 111222333, Nome = "Jakim das Coives S.A.", Email = "jakimdascoives@upskill.pt" },
                new Cliente { Nif = 444555666, Nome = "Manel dos PC's", Email = "maneldospcs@upskill.pt" },
                new Cliente { Nif = 777888999, Nome = "Toni das Beringelas", Email = "toninhodasberingelas@upskill.pt" },
                new Cliente { Nif = 111000111, Nome = "Pico da Programação", Email = "piquinhodosprogramas@upskill.pt" }
            );
            dbContext.SaveChanges();

            dbContext.Freguesias.AddRange( //apenas uma freguesia
                new Freguesia { Nome = "Custóias" }
            );
            dbContext.SaveChanges();

            dbContext.Parques.AddRange(
                new Parque { Rua = "Lugar da Coive", FreguesiaId = 1 }
            );
            dbContext.SaveChanges();

            dbContext.Lugares.AddRange(
                new Lugar { Numero = 1, Fila = "A", Andar = -2, ParqueId = 1, Preco = 4.99 },
                new Lugar { Numero = 2, Fila = "B", Andar = -1, ParqueId = 1, Preco = 5.99 },
                new Lugar { Numero = 3, Fila = "C", Andar = 0, ParqueId = 1, Preco = 6.99 },
                new Lugar { Numero = 4, Fila = "D", Andar = 1, ParqueId = 1, Preco = 7.99 },
                new Lugar { Numero = 5, Fila = "E", Andar = 2, ParqueId = 1, Preco = 99.99 }
            );
            dbContext.SaveChanges();

            dbContext.Reservas.AddRange(
                new Reserva { NifCliente = 111222333, LugarId = 1, Inicio = DateTime.Parse("2021-01-30 11:00:00"), Fim = DateTime.Parse("2021-01-30 12:00:00") },
                new Reserva { NifCliente = 444555666, LugarId = 2, Inicio = DateTime.Parse("2021-01-29 21:00:00"), Fim = DateTime.Parse("2021-01-31 10:00:00") },
                new Reserva { NifCliente = 777888999, LugarId = 3, Inicio = DateTime.Parse("2021-01-30 10:00:00"), Fim = DateTime.Parse("2021-01-30 13:00:00") },
                new Reserva { NifCliente = 111000111, LugarId = 4, Inicio = DateTime.Parse("2021-01-30 11:00:00"), Fim = DateTime.Parse("2021-01-30 13:00:00") }
            );

            dbContext.SaveChangesAsync();
        }
    }
}
