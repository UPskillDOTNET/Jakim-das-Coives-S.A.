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
    public class TestFreguesiasController
    {
        [Fact]

        public async Task GetAllFreguesias()
        {
            //Arrange
            var TestContext = API_Parque_PublicoContextMocker.GetPubParkContext("DBTest4GetAll");
            var TestController = new FreguesiasController(TestContext);

            //Act
            var result = await TestController.GetFreguesia();

            //Assert
            var items = Assert.IsType<List<Freguesia>>(result.Value);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public async Task GetFreguesiabyId()
        {
            //Arrange
            var TestContext = API_Parque_PublicoContextMocker.GetPubParkContext("DBTestForGetId");
            var TestController = new FreguesiasController(TestContext);

            //Act
            var result = await TestController.GetFreguesia(1);

            //Assert
            var items = Assert.IsType<Freguesia>(result.Value);
            Assert.Equal(1, items.Id);
        }

        //POST
        [Fact]
        public async Task PostFreguesia_CreateNewFreguesia()
        {
            //Arrange
            var testContext = API_Parque_PublicoContextMocker.GetPubParkContext("Post4Freguesia");
            var TestController = new FreguesiasController(testContext);

            //Act
            var result = await TestController.PostFreguesia(new Freguesia { Id = 3, Nome = "Coives" });
            var get = await TestController.GetFreguesia(3);

            //Assert
            Assert.IsType<Freguesia>(get.Value);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task DeleteFreguesiaById()
        {
            //Arrange
            var testContext = API_Parque_PublicoContextMocker.GetPubParkContext("DeleteFreguesia");
            var TestController = new FreguesiasController(testContext);

            //Act
            var result = await TestController.DeleteFreguesia(2);
            var get = await TestController.GetFreguesia(2);

            //Assert
            Assert.IsType<NotFoundResult>(get.Result);
            Assert.IsType<NoContentResult>(result);
        }

        //PUT
        [Fact]
        public async Task PutFreguesiaById()
        {
            //Arrange
            var testContext = API_Parque_PublicoContextMocker.GetPubParkContext("PutFreguesia");
            var TestController = new FreguesiasController(testContext);

            //Act
            var getFreguesia = await TestController.GetFreguesia(1);
            var freguesia = getFreguesia.Value;
            freguesia.Id = 1;
            freguesia.Nome = "Batatas";
            var result = await TestController.PutFreguesia(freguesia.Id, freguesia);
            var getresult = await TestController.GetFreguesia(1);

            //Assert
            var items = Assert.IsType<Freguesia>(getresult.Value);
            Assert.Equal(1, items.Id);
            Assert.Equal("Batatas", items.Nome);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
