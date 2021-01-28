using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using API_Parque_Publico.Controllers;
using API_Parque_Publico.Models;

namespace UnitTest.Unit_Tests
{
    public class TestReservasController
    {
        [Fact]

        public async Task GetAllReservas()
        {
            //Arrange
            var TestContext = API_Parque_PublicoContextMocker.GetPubParkContext("DBTest4GetAll");
            var TestController = new ReservasController(TestContext);

            //Act
            var result = await TestController.GetReserva();

            //Assert
            var items = Assert.IsType<List<Reserva>>(result.Value);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public async Task GetReservabyId()
        {
            //Arrange
            var TestContext = API_Parque_PublicoContextMocker.GetPubParkContext("DBTestForGetId");
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
            var testContext = API_Parque_PublicoContextMocker.GetPubParkContext("Post4Reserva");
            var TestController = new ReservasController(testContext);

            //Act
            var result = await TestController.PostReserva(new Reserva { Id = 3, NifCliente = 123123123, LugarId = 5, Inicio = DateTime.Parse("2021-01-01 10:10:10"), Fim = DateTime.Parse("2031-01-01 10:10:10") });
            var get = await TestController.GetReserva(3);

            //Assert
            Assert.IsType<Reserva>(get.Value);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task DeleteReservaById()
        {
            //Arrange
            var testContext = API_Parque_PublicoContextMocker.GetPubParkContext("DeleteReserva");
            var TestController = new ReservasController(testContext);

            //Act
            var result = await TestController.DeleteReserva(2);
            var get = await TestController.GetReserva(2);

            //Assert
            Assert.IsType<NotFoundResult>(get.Result);
            Assert.IsType<NoContentResult>(result);
        }

        //PUT
        [Fact]
        public async Task PutReservaById()
        {
            //Arrange
            var testContext = API_Parque_PublicoContextMocker.GetPubParkContext("PutReserva");
            var TestController = new ReservasController(testContext);

            //Act
            var getReserva = await TestController.GetReserva(1);
            var reserva = getReserva.Value;
            reserva.Id = 1;
            reserva.NifCliente = 321321321;
            reserva.LugarId = 9;
            reserva.Inicio = DateTime.Parse("1021-01-01 10:10:10");
            reserva.Fim = DateTime.Parse("3021-01-01 10:10:10");
            var result = await TestController.PutReserva(reserva.Id, reserva);
            var getresult = await TestController.GetReserva(1);

            //Assert
            var items = Assert.IsType<Reserva>(getresult.Value);
            Assert.Equal(1, items.Id);
            Assert.Equal(321321321, items.NifCliente);
            Assert.Equal(9, items.LugarId);
            Assert.Equal(DateTime.Parse("1021-01-01 10:10:10"), items.Inicio);
            Assert.Equal(DateTime.Parse("3021-01-01 10:10:10"), items.Fim);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
