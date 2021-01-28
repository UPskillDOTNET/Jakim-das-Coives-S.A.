using API_Parque_Publico.Data;
using API_Parque_Publico.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    class API_Parque_PublicoContextMocker
    {
        private static API_Parque_PublicoContext dbContext;
        public static API_Parque_PublicoContext GetPubParkContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<API_Parque_PublicoContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
            dbContext = new API_Parque_PublicoContext(options);
            Seed();
            return dbContext;
        }

        private static void Seed()
        {
            dbContext.Clientes.Add(new Cliente
            {
                Nif = 123456789,
                Nome = "Jakim das Coives S.A.",
                Email = "help@jakimdascoives.pt"
            });
            dbContext.Clientes.Add(new Cliente
            {
                Nif = 987654321,
                Nome = "Jakim",
                Email = "help@jakimdascoives.pt"
            });
            dbContext.Freguesias.Add(new Freguesia
            {
                Id = 1,
                Nome = "Jakim das Coives",
            });
            dbContext.Freguesias.Add(new Freguesia
            {
                Id = 2,
                Nome = "Jakim",
            });
            dbContext.Parques.Add(new Parque
            {
                Id = 1,
                Rua = "sao bento",
                FreguesiaId = 1
            });
            dbContext.Parques.Add(new Parque
            {
                Id = 2,
                Rua = "Campanha",
                FreguesiaId = 1
            });
            dbContext.Lugares.Add(new Lugar
            {
                Id = 1,
                ParqueId = 1,
                Numero = 1,
                Fila = "1",
                Andar = 1,
                Preco = 1
            });
            dbContext.Lugares.Add(new Lugar
            {
                Id = 2,
                ParqueId = 1,
                Numero = 1,
                Fila = "2",
                Andar = 1,
                Preco = 1
            });
            dbContext.Reservas.Add(new Reserva
            {
                Id = 1,
                NifCliente = 123456789,
                LugarId = 1,
                Inicio = DateTime.Parse("2021-01-01 10:10:10"),
                Fim = DateTime.Parse("2021-01-02 10:10:10")
            });
            dbContext.Reservas.Add(new Reserva
            {
                Id = 2,
                NifCliente = 987654321,
                LugarId = 1,
                Inicio = DateTime.Parse("2021-01-01 10:10:10"),
                Fim = DateTime.Parse("2021-01-02 10:10:10")
            });

            dbContext.SaveChangesAsync();
        }
    }
}
