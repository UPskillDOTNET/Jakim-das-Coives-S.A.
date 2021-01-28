using API_Parque_Publico.Controllers;
using API_Parque_Publico.Models;
using Microsoft.AspNetCore.Mvc;
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

        [Fact]
        public async Task GetLugarById()
        {
            //arrange
            var TestContext = API_Parque_PublicoContextMocker.GetPubParkContext("DBtestGetLugarById");
            var TestController = new LugaresController(TestContext);

            //Act
            var Result = await TestController.GetLugar(2);

            //Assert
            var items = Assert.IsType<Lugar>(Result.Value);
            Assert.Equal(2, items.Id);
        }

        [Fact]
        public async Task PostLugar_CreateNewLugar()
        {
            //arrange
            var TestContext = API_Parque_PublicoContextMocker.GetPubParkContext("DBtestPostLugar");
            var TestController = new LugaresController(TestContext);

            //Act
            var Result = await TestController.PostLugar(new Lugar { Id = 3, ParqueId = 1, Numero = 1, Fila = "1", Andar = 1, Preco = 1});
            var get = await TestController.GetLugar(3);

            //Assert
            Assert.IsType<Lugar>(get.Value);
            Assert.IsType<CreatedAtActionResult>(Result.Result);
        }
        [Fact]
        public async Task DeleteLugar()
        {
            //arrange
            var TestContext = API_Parque_PublicoContextMocker.GetPubParkContext("DBtestDeleteLugar");
            var TestController = new LugaresController(TestContext);

            //Act
            var Result = await TestController.DeleteLugar(1);
            var get = await TestController.GetLugar(1);

            //Assert
            Assert.IsType<NotFoundResult>(get.Result);
            Assert.IsType<NoContentResult>(Result);
        }
        [Fact]
        public async Task PutLugar()
        {
            //Arrange
            var testContext = API_Parque_PublicoContextMocker.GetPubParkContext("PutLugar");
            var TestController = new LugaresController(testContext);

            //Act
            var lugar = new Lugar { Id = 1, ParqueId = 1, Numero = 3, Fila = "1", Andar = 1, Preco = 1 };
            var getLugar = await TestController.GetLugar(1);
            var cc = getLugar.Value;
            testContext.Entry(cc).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            var Result = await TestController.PutLugar(lugar.Id, lugar);
            var getResult = await TestController.GetLugar(1);

            //Assert
            var items = Assert.IsType<Lugar>(getResult.Value);
            Assert.IsType<NoContentResult>(Result);
        }
        [Fact]
        public void GetAvailableLugares()
        {
            //Arrange
            var testContext = API_Parque_PublicoContextMocker.GetPubParkContext("GetLugaresAvailable");
            var TestController = new LugaresController(testContext);

            //Act
            var Result = TestController.FindAvailable(1, DateTime.Parse("2021-01-01 10:10:10"), DateTime.Parse("2021-01-02 10:10:10"));

            //Assert
            var items = Assert.IsAssignableFrom<IEnumerable<Lugar>>(Result);
            List<Lugar> lista = items.ToList();
            Assert.Equal(2, lista.Count);

        }
    }
}

