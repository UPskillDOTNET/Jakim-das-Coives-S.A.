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

namespace SCUnitTests
{
    public class SubAlugueresControllerTest
    {
        [Fact]
        public async void GetAllSubAluguerByNif_ShouldReturnMultipleSubAlugueresFrom999999999()
        {
            //Arrange
            var mock = new Mock<ISubAluguerService>();
            mock.Setup(s => s.GetByNifAsync("999999999"))
                .ReturnsAsync(new List<SubAluguerDTO>{
                    new SubAluguerDTO
                    {
                        Id = 1,
                        ParqueId = 1,
                        Numero = 1,
                        Fila = "A",
                        Andar = 0,
                        Preco = 4.99,
                        NifProprietario = "999999999",
                        Inicio = DateTime.Parse("2021-02-01T12:00:00"),
                        Fim = DateTime.Parse("2021-02-01T13:00:00"),
                    }
            });

            SubAlugueresController testController = new SubAlugueresController(mock.Object);

            //Act
            var result = await testController.GetSubAluguerByNif("999999999");

            //Assert
            var items = Assert.IsType<List<SubAluguerDTO>>(result.Value);

        }
    }
}
