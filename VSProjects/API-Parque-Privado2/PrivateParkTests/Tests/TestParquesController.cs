using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using API_Parque_Privado2.Controllers;
using API_Parque_Privado2.Models;

namespace UnitTest.Unit_Tests
{
    public class TestParquesController
    {
        [Fact]

        public async Task GetAllParques_ShouldOnlyReturnOne() // apenas deve retornar um, o próprio parque privado
        {
            //Arrange
            var TestContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBTest4GetAllParques");
            var TestController = new ParquesController(TestContext);

            //Act
            var result = await TestController.GetParque();

            //Assert
            var items = Assert.IsType<List<Parque>>(result.Value);
            Assert.Single(items);
        }

        [Fact]
        public async Task GetParquebyId_ShouldReturnFirstParque()
        {
            //Arrange
            var TestContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBTestForGetIdParque");
            var TestController = new ParquesController(TestContext);

            //Act
            var result = await TestController.GetParque(1);

            //Assert
            var item = Assert.IsType<Parque>(result.Value);
            Assert.Equal(1, item.Id);
        }

        //POST
        [Fact]
        public async Task PostParque_CreateNewParque()
        {
            //Arrange
            var testContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBTestPost4Parque");
            var TestController = new ParquesController(testContext);

            //Act
            var result = await TestController.PostParque(new Parque { Id = 2, Rua = "Jakim Coives Jr.", FreguesiaId = 1 });
            var get = await TestController.GetParque(1);

            //Assert
            Assert.IsType<Parque>(get.Value);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task DeleteParqueById_ShouldDeleteFirstParque()
        {
            //Arrange
            var testContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBTestDeleteParque");
            var TestController = new ParquesController(testContext);

            //Act
            var result = await TestController.DeleteParque(1);
            var get = await TestController.GetParque(1);

            //Assert
            Assert.IsType<NotFoundResult>(get.Result);
            Assert.IsType<NoContentResult>(result);
        }

        //PUT
        [Fact]
        public async Task PutParqueById_ShouldUpdateFirstParque()
        {
            //Arrange
            var testContext = API_Parque_PrivadoContextMocker.GetPrivParkContext("DBTestPutFreguesia");
            var TestController = new ParquesController(testContext);

            //Act
            var getParque = await TestController.GetParque(1);
            var parque = getParque.Value;
            parque.Id = 1;
            parque.Rua = "Praça da Coive";
            parque.FreguesiaId = 1;
            var result = await TestController.PutParque(parque.Id, parque);
            var getresult = await TestController.GetParque(1);

            //Assert
            var items = Assert.IsType<Parque>(getresult.Value);
            Assert.Equal(1, items.Id);
            Assert.Equal("Praça da Coive", items.Rua);
            Assert.Equal(1, items.FreguesiaId);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
