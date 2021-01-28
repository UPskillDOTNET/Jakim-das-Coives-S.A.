using API_Parque_Publico.Controllers;
using API_Parque_Publico.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Unit_Tests
{
    public class TestClientesController
    {
        [Fact]
        public async Task GetAllClientesAsync_ShouldReturnAllClientsAsync() {
            //Arrange

            var TestContext = API_Parque_PublicoContextMocker.GetPubParkContext("DBGetAllClients");
            var TestController = new ClientesController(TestContext);

            //Act

            var result = await TestController.GetCliente();

            //Assert

            var items = Assert.IsType<List<Cliente>>(result.Value);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public async Task GetClientesByNifAsync_ShouldReturnJakimDasCoivesSA()
        {
            //Arrange

            var TestContext = API_Parque_PublicoContextMocker.GetPubParkContext("DBGetClienteByNif");
            var TestController = new ClientesController(TestContext);

            //Act

            var result = await TestController.GetCliente(123456789);

            //Assert

            var item = Assert.IsType<Cliente>(result.Value);
            Assert.Equal("Jakim das Coives S.A.", item.Nome);
            Assert.Equal("help@jakimdascoives.pt", item.Email);
        }

        [Fact]
        public async Task PostClienteAsync_ShouldReturnCreatedAsync()
        {
            //Arrange

            var TestContext = API_Parque_PublicoContextMocker.GetPubParkContext("DBPostCliente");
            var TestController = new ClientesController(TestContext);


            //Act

            var result = await TestController.PostCliente(new Cliente { Nif = 123456781, Nome = "testClient", Email = "testemail@jakimdascoives.pt" });
            var get = await TestController.GetCliente(123456781);

            //Assert

            var item = Assert.IsType<Cliente>(get.Value);
            Assert.Equal("testClient", item.Nome);
            Assert.Equal("testemail@jakimdascoives.pt", item.Email);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task PutClienteAsync_ShouldUpdateClientJakimDasCoivesAsync()
        {
            //Arrange
            var TestContext = API_Parque_PublicoContextMocker.GetPubParkContext("DBPutCliente");
            var TestController = new ClientesController(TestContext);

            //Act
            var client = new Cliente
            {
                Nif = 123456789,
                Nome = "changedName",
                Email = "changedemail@jakimdascoives.pt"
            };
            var getClient = await TestController.GetCliente(123456789);
            var c1 = getClient.Value;
            TestContext.Entry(c1).State = EntityState.Detached;
            var result = await TestController.PutCliente(123456789, client);
            var getresult = await TestController.GetCliente(123456789);

            //Assert
            Assert.IsType<NoContentResult>(result);
            var item = Assert.IsType<Cliente>(getresult.Value);
            Assert.Equal(123456789, item.Nif);
            Assert.Equal("changedName", item.Nome);
            Assert.Equal("changedemail@jakimdascoives.pt", item.Email);
        }

        [Fact]
        public async Task DeleteClienteAsync_ShouldDeleteClienteJakimDasCoivesAsync()
        {
            //Arrange
            var TestContext = API_Parque_PublicoContextMocker.GetPubParkContext("DBDeleteCliente");
            var TestController = new ClientesController(TestContext);

            //Act
            var result = await TestController.DeleteCliente(123456789);
            var get = await TestController.GetCliente(123456789);

            //Assert
            Assert.IsType<NotFoundResult>(get.Result);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
