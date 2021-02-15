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
                    },
                    new SubAluguerDTO
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
                    },
                    new SubAluguerDTO
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
                    },
                    new SubAluguerDTO
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
                    }

            });

            SubAlugueresController testController = new SubAlugueresController(mock.Object);

            //Act
            var result = await testController.GetSubAluguerByNif("999999999");

            //Assert
            var items = Assert.IsType<List<SubAluguerDTO>>(result.Value);
            Assert.Equal(4, items.Count);
            foreach (SubAluguerDTO a in items)
            {
                Assert.Equal("999999999", a.NifProprietario);
            }
        }
        [Fact]
        public async void GetSubAluguerById_ShouldReturnSubAluguerFromId1()
        {
            //Arrange
            var mock = new Mock<ISubAluguerService>();
            mock.Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(new SubAluguerDTO
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
                });

            SubAlugueresController testController = new SubAlugueresController(mock.Object);

            //Act
            var result = await testController.GetSubAluguerById(1);

            //Assert
            var item = Assert.IsType<SubAluguerDTO>(result.Value);
            Assert.Equal(1, item.Id);
        }

        [Fact]
        public async Task PostSubAluguer_ShoulCreateNewSubAluguer()
        {
            //Arrange
            var mock = new Mock<ISubAluguerService>();
            SubAlugueresController testController = new SubAlugueresController(mock.Object);
            var model = new SubAluguerDTO
            {
                ParqueId = 1,
                Numero = 10,
                Fila = "A",
                Andar = 0,
                Preco = 5,
                NifProprietario = "111111111",
                ReservaOriginalId = 1,
                Inicio = DateTime.Parse("2021-02-01 12:00:00"),
                Fim = DateTime.Parse("2021-02-01 13:00:00"),
            };
            //Act
            var result = await testController.PostSubAluguer(model);
           // var get = await controller.GetSubAluguerById(10);

            //Assert
            Assert.IsType<SubAluguerDTO>(result.Value);
           // Assert.IsType<SubAluguerDTO>(get.Value);
            //Assert.IsType<CreatedAtActionResult>(result.Result);
            //Assert.Equal(10, result.Value.Id);
            // Assert.Equal(10, get.Value.Id);
            // Assert.Equal("A", items.Fila);
            // Assert.Equal(4.99, items.Preco);
            // Assert.Equal(DateTime.Parse("2021-02-01T13:00:00"), items.Fim);

        }
        [Fact]
        public async Task DeleteSubAluguer_ShoulDeleteSubAluguer()
        {
            //Arrange
            var mock = new Mock<ISubAluguerService>();
            mock.Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(new SubAluguerDTO
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
                });

            SubAlugueresController TestController = new SubAlugueresController(mock.Object);

            //Act
            var result = await TestController.DeleteSubAluguer(1);
           // var get = await TestController.GetSubAluguerById(1);
            //Assert
            Assert.IsType<NoContentResult>(result);
        //    Assert.IsType<NotFoundResult>(get.Result);

        }
    }
}

