using API_Parque_Publico.Controllers;
using API_Parque_Publico.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Unit_Tests
{
    public class TestLugaresController
    {
        [Fact]
        public async Task GetAllLugares()
        {
            //arrange
            var TestContext = API_Parque_PublicoContextMocker.GetPubParkContext("DBtestGetAll");
            var TestController = new LugaresController(TestContext);

            //Act
            var Result = await TestController.GetLugar();

            //Assert
            var items = Assert.IsType<List<Lugar>>(Result.Value);
            Assert.Equal(2, items.Count);
        }
    }
}
