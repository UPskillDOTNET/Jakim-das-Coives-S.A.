using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using API_Parque_Privado_3.Controllers;
using API_Parque_Privado_3.Models;
using System.Threading;

namespace UnitTest.Unit_Tests
{
    public class TestFreguesiasController
    {
        [Fact]

        public async Task GetAllFreguesias_ShouldReturnOnlyOneFreguesia()
        {
            //Arrange
            var TestContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBTest4GetAllFreguesias");
            var TestController = new FreguesiasController(TestContext);

            //Act
            var result = await TestController.GetFreguesia();

            //Assert
            var items = Assert.IsType<List<Freguesia>>(result.Value);
            Assert.Single(items);
        }

        [Fact]
        public async Task GetFreguesiabyId_ShouldReturnFirstFreguesia()
        {
            //Arrange
            var TestContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBTestForGetIdFreg");
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
            var testContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("Post4Freguesia");
            var TestController = new FreguesiasController(testContext);

            //Act
            var result = await TestController.PostFreguesia(new Freguesia { Id = 6, Nome = "Coives" });
            var get = await TestController.GetFreguesia(6);

            //Assert
            Assert.IsType<Freguesia>(get.Value);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task DeleteFreguesiaById_TestNoContent_NotFound()
        {
            //Arrange
            var testContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DeleteFreguesia");
            var TestController = new FreguesiasController(testContext);

            //Act
            var result = await TestController.DeleteFreguesia(1);
            var get = await TestController.GetFreguesia(1);

            //Assert
            Assert.IsType<NotFoundResult>(get.Result);
            Assert.IsType<NoContentResult>(result);
        }

        //PUT
        [Fact]
        public async Task PutFreguesiaById_UpdateFirstFreguesia()
        {
            //Arrange
            var testContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("PutFreguesia");
            var TestController = new FreguesiasController(testContext);

            //Act
            var getFreguesia = await TestController.GetFreguesia(1);
            var freguesia = getFreguesia.Value;
            freguesia.Id = 1;
            freguesia.Nome = "Paranhos";
            var result = await TestController.PutFreguesia(freguesia.Id, freguesia);
            var getresult = await TestController.GetFreguesia(1);

            //Assert
            var items = Assert.IsType<Freguesia>(getresult.Value);
            Assert.Equal(1, items.Id);
            Assert.Equal("Paranhos", items.Nome);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
