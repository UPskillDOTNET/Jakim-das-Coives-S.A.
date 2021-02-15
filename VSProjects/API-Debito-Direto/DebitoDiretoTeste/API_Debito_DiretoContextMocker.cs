using System;
using Xunit;
using Api


namespace DebitoDiretoTeste
{
    public class API_Debito_DiretoContextMocker
    {
        private static Api_DebitoDiretoContext dbContext;
        public static API_Parque_Privado2Context GetPrivParkContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<API_Parque_Privado2Context>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
            dbContext = new API_Parque_Privado2Context(options);
            Seed();
            return dbContext;
        }
        [Fact]
        public void Test1()
        {

        }
    }
}
