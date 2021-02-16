
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using API_Sistema_Central.Models;
using API_Sistema_Central.Controllers;
using API_Sistema_Central.Services;
using API_Sistema_Central.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace SCUnitTests
{
    [Collection("Sequential")]
    public class SubAlugueresControllerTest
    {
        ISubAluguerService serviceMock;
        public SubAlugueresControllerTest()
        {

            var SubAluguerList = new List<SubAluguerDTO>();
            SubAluguerList.Add(new SubAluguerDTO()
            {
                Id = 1,
                ParqueId = 1,
                Numero = 1,
                Fila = "A",
                Andar = 0,
                Preco = 4.99,
                NifProprietario = "222222222",
                Inicio = DateTime.Parse("2021-02-01T12:00:00"),
                Fim = DateTime.Parse("2021-02-01T13:00:00"),
            });
            SubAluguerList.Add(new SubAluguerDTO()
            {
                Id = 2,
                ParqueId = 1,
                Numero = 1,
                Fila = "A",
                Andar = 0,
                Preco = 4.99,
                NifProprietario = "999999999",
                Inicio = DateTime.Parse("2021-02-01T13:00:00"),
                Fim = DateTime.Parse("2021-02-01T14:00:00"),
            });
            SubAluguerList.Add(new SubAluguerDTO()
            {
                Id = 3,
                ParqueId = 2,
                Numero = 1,
                Fila = "A",
                Andar = 0,
                Preco = 4.99,
                NifProprietario = "999999999",
                Inicio = DateTime.Parse("2021-02-01T12:00:00"),
                Fim = DateTime.Parse("2021-02-01T13:00:00"),
            });
            SubAluguerList.Add(new SubAluguerDTO()
            {
                Id = 4,
                ParqueId = 2,
                Numero = 2,
                Fila = "A",
                Andar = 0,
                Preco = 4.99,
                NifProprietario = "999999999",
                Inicio = DateTime.Parse("2021-02-01T12:00:00"),
                Fim = DateTime.Parse("2021-02-01T13:00:00"),
            });


            var mock = new Mock<ISubAluguerService>();

            mock.Setup(s => s.GetByNifAsync("999999999")).ReturnsAsync(new List<SubAluguerDTO> { SubAluguerList[1], SubAluguerList[2], SubAluguerList[3] });
            mock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(SubAluguerList[0]);
            mock.Setup(x => x.PostSubAluguerAsync(It.IsAny<SubAluguerDTO>())).ReturnsAsync(new SubAluguerDTO
            {
                Id = 5,
                ParqueId = 2,
                Numero = 2,
                Fila = "B",
                Andar = 0,
                Preco = 4.99,
                NifProprietario = "999999999",
                Inicio = DateTime.Parse("2021-02-01T12:00:00"),
                Fim = DateTime.Parse("2021-02-01T13:00:00"),
            });
            
            serviceMock = mock.Object;
        }


        [Fact]
        public async Task GetSubAluguerbyNif_ShouldTeturnAll9999999999()
        {
            // Arrange
            var theController = new SubAlugueresController(serviceMock);

            // Act
            var result = await theController.GetSubAluguerByNif("999999999");

            // Assert
            var subAlugueres = Assert.IsType<List<SubAluguerDTO>>(result.Value);
            Assert.Equal(3, subAlugueres.Count());
        }

        [Fact]
        public async Task GetSubAluguerById_ShouldReturnOneSubAluguer()
        {
            // Arrange       
            var theController = new SubAlugueresController(serviceMock);
            var testCod = 1;

            // Act
            var result = await theController.GetSubAluguerById(testCod);

            //Assert     
            Assert.IsType<SubAluguerDTO>(result.Value);

        }
        [Fact]
        public async Task DeleteExistingSubAluguerAsync_ShouldRemoveSubAluguer()
        {
            // Arrange       
            var theController = new SubAlugueresController(serviceMock);
            var testCod = 1;

            // Act
            var result = await theController.DeleteSubAluguer(testCod);


            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task PostCountryAsync_ShouldCreateAnCountryAsync()
        {
           // Arrange       
            var theController = new SubAlugueresController(serviceMock);
            var newSubAluguer = new SubAluguerDTO
            {
                ParqueId = 2,
                Numero = 2,
                Fila = "B",
                Andar = 0,
                Preco = 4.99,
                NifProprietario = "999999999",
                Inicio = DateTime.Parse("2021-02-01T12:00:00"),
                Fim = DateTime.Parse("2021-02-01T13:00:00"),
            };

            // Act
            var response = await theController.PostSubAluguer(newSubAluguer);
            var result = response.Result as CreatedAtActionResult;

            // Assert
            Assert.IsNotType<BadRequestObjectResult>(result);
            Assert.NotNull(response);
            var value = GetObjectResultContent(response);
            Assert.IsType<SubAluguerDTO>(value);
            Assert.Equal(5, value.Id);
        }
        private static T GetObjectResultContent<T>(ActionResult<T> result)
        {
            return (T)((ObjectResult)result.Result).Value;
        }
    }
}
