using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using API_Parque_Privado2.Controllers;
using API_Parque_Privado2.Models;

namespace UnitTest.Unit_Tests
{
    public class TestReservasController
    {
        [Fact]

        public async Task GetAllReservas_ShouldReturnAllReservas()
        {
            //Arrange
            var TestContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBTest4GetAllReservas");
            var TestController = new ReservasController(TestContext);

            //Act
            var result = await TestController.GetReserva();

            //Assert
            var items = Assert.IsType<List<Reserva>>(result.Value);
            Assert.Equal(4, items.Count);
        }

        [Fact]
        public async Task GetReservabyId_ShouldReturnFirstReserva()
        {
            //Arrange
            var TestContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBTestForGetIdReserva");
            var TestController = new ReservasController(TestContext);

            //Act
            var result = await TestController.GetReserva(1);

            //Assert
            var items = Assert.IsType<Reserva>(result.Value);
            Assert.Equal(1, items.Id);
        }

        //POST
        [Fact]
        public async Task PostReserva_CreateNewReserva()
        {
            //Arrange
            var testContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBTestPost4Reserva");
            var TestController = new ReservasController(testContext);

            //Act
            var result = await TestController.PostReserva(new Reserva { Id = 5, NifCliente = 111222333, LugarId = 5, Inicio = DateTime.Parse("2021-01-01 10:10:10"), Fim = DateTime.Parse("2031-01-01 10:10:10") });
            var get = await TestController.GetReserva(5);

            //Assert
            Assert.IsType<Reserva>(get.Value);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task DeleteReservaById_ShouldDeleteFirstReserva()
        {
            //Arrange
            var testContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBTestDeleteReserva");
            var TestController = new ReservasController(testContext);

            //Act
            var result = await TestController.DeleteReserva(1);
            var get = await TestController.GetReserva(1);

            //Assert
            Assert.IsType<NotFoundResult>(get.Result);
            Assert.IsType<NoContentResult>(result);
        }

        //PUT
        [Fact]
        public async Task PutReservaById_ShouldUpdateReservaOne()
        {
            //Arrange
            var testContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBTestPutReserva");
            var TestController = new ReservasController(testContext);

            //Act
            var getReserva = await TestController.GetReserva(1);
            var reserva = getReserva.Value;
            reserva.Id = 1;
            reserva.NifCliente = 111222333;
            reserva.LugarId = 1;
            reserva.Inicio = DateTime.Parse("2021-01-01 10:10:10");
            reserva.Fim = DateTime.Parse("2021-03-01 10:10:10");
            var result = await TestController.PutReserva(reserva.Id, reserva);
            var getresult = await TestController.GetReserva(1);

            //Assert
            var items = Assert.IsType<Reserva>(getresult.Value);
            Assert.Equal(1, items.Id);
            Assert.Equal(111222333, items.NifCliente);
            Assert.Equal(1, items.LugarId);
            Assert.Equal(DateTime.Parse("2021-01-01 10:10:10"), items.Inicio);
            Assert.Equal(DateTime.Parse("2021-03-01 10:10:10"), items.Fim);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
