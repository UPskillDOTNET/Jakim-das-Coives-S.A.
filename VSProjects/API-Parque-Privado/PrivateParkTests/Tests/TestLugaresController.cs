using API_Parque_Privado.Controllers;
using API_Parque_Privado.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Unit_Tests
{
    public class TestLugaresController
    {
        [Fact]
        public async Task GetAllLugares_ShouldReturnAllLugares()
        {
            //arrange
            var TestContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBtestGetAllLugares");
            var TestController = new LugaresController(TestContext);

            //Act
            var Result = await TestController.GetLugar();

            //Assert
            var items = Assert.IsType<List<Lugar>>(Result.Value);
            Assert.Equal(5, items.Count);
        }

        [Fact]
        public async Task GetLugarById_ShouldReturnLugarTwo()
        {
            //arrange
            var TestContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBtestGetLugarById");
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
            var TestContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBtestPostLugar");
            var TestController = new LugaresController(TestContext);

            //Act
            var Result = await TestController.PostLugar(new Lugar { Numero = 6, Fila = "F", Andar = 3, ParqueId = 1, Preco = 100000.99 });
            var get = await TestController.GetLugar(6);

            //Assert
            Assert.IsType<Lugar>(get.Value);
            Assert.IsType<CreatedAtActionResult>(Result.Result);
        }
        [Fact]
        public async Task DeleteLugar_ShouldDeleteFirstLugar()
        {
            //arrange
            var TestContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBtestDeleteLugar");
            var TestController = new LugaresController(TestContext);

            //Act
            var Result = await TestController.DeleteLugar(1);
            var get = await TestController.GetLugar(1);

            //Assert
            Assert.IsType<NotFoundResult>(get.Result);
            Assert.IsType<NoContentResult>(Result);
        }
        [Fact]
        public async Task PutLugar_ShouldUpdateLugarOne()
        {
            //Arrange
            var testContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBtestPutLugar");
            var TestController = new LugaresController(testContext);

            //Act
            var lugar = new Lugar { Id = 1, ParqueId = 1, Numero = 3, Fila = "1", Andar = 1, Preco = 1 };
            var getLugar = await TestController.GetLugar(1);
            var cc = getLugar.Value;
            testContext.Entry(cc).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            var Result = await TestController.PutLugar(lugar.Id, lugar);
            var getResult = await TestController.GetLugar(1);

            //Assert
            Assert.IsType<Lugar>(getResult.Value);
            Assert.IsType<NoContentResult>(Result);
        }
        [Fact]
        public void GetAvailableLugares_ShouldReturnOneLugarWithoutReservations()
        {
            //Arrange
            var testContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBtestGetLugaresAvailable");
            var TestController = new LugaresController(testContext);

            //Act
            var Result = TestController.FindAvailable(1, DateTime.Parse("2021-01-30 11:00:00"), DateTime.Parse("2021-01-30 12:00:00"));

            //Assert
            var items = Assert.IsAssignableFrom<IEnumerable<Lugar>>(Result);
            List<Lugar> lista = items.ToList();
            Assert.Single(lista);

        }
    }
}

