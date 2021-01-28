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
    public class TestParquesController
    {
        [Fact]

        public async Task GetAllParques()
        {
            //Arrange
            var TestContext = API_Parque_PublicoContextMocker.GetPubParkContext("DBTest4GetAll");
            var TestController = new ParquesController(TestContext);

            //Act
            var result = await TestController.GetParque();

            //Assert
            var items = Assert.IsType<List<Parque>>(result.Value);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public async Task GetParquebyId()
        {
            //Arrange
            var TestContext = API_Parque_PublicoContextMocker.GetPubParkContext("DBTestForGetId");
            var TestController = new ParquesController(TestContext);

            //Act
            var result = await TestController.GetParque(1);

            //Assert
            var items = Assert.IsType<Parque>(result.Value);
            Assert.Equal(1, items.Id);
        }

        //POST
        [Fact]
        public async Task PostParque_CreateNewParque()
        {
            //Arrange
            var testContext = API_Parque_PublicoContextMocker.GetPubParkContext("Post4Parque");
            var TestController = new ParquesController(testContext);

            //Act
            var result = await TestController.PostParque(new Parque { Id = 3, Rua = "Santa Apolonia", FreguesiaId = 2 });
            var get = await TestController.GetParque(3);

            //Assert
            Assert.IsType<Parque>(get.Value);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task DeleteParqueById()
        {
            //Arrange
            var testContext = API_Parque_PublicoContextMocker.GetPubParkContext("DeleteParque");
            var TestController = new ParquesController(testContext);

            //Act
            var result = await TestController.DeleteParque(2);
            var get = await TestController.GetParque(2);

            //Assert
            Assert.IsType<NotFoundResult>(get.Result);
            Assert.IsType<NoContentResult>(result);
        }

        //PUT
        [Fact]
        public async Task PutParqueById()
        {
            //Arrange
            var testContext = API_Parque_PublicoContextMocker.GetPubParkContext("PutFreguesia");
            var TestController = new ParquesController(testContext);

            //Act
            var getParque = await TestController.GetParque(1);
            var parque = getParque.Value;
            parque.Id = 1;
            parque.Rua = "braga";
            parque.FreguesiaId = 2;
            var result = await TestController.PutParque(parque.Id, parque);
            var getresult = await TestController.GetParque(1);

            //Assert
            var items = Assert.IsType<Parque>(getresult.Value);
            Assert.Equal(1, items.Id);
            Assert.Equal("braga", items.Rua);
            Assert.Equal(2, items.FreguesiaId);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
