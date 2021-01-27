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
        }
    }
}
