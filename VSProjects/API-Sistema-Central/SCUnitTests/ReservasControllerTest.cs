using API_Sistema_Central.Services;
using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using API_Sistema_Central.Models;
using API_Sistema_Central.Controllers;
using API_Sistema_Central.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace SCUnitTests
{
    public class ReservasControllerTest
    {
        [Fact]
        public async void GetAllReservasByNif_ShouldReturnMultipleReservasFrom999999999()
        {
            var mock = new Mock<IReservaService>();
            mock.Setup(p => p.GetByNifAsync("999999999"))
                .ReturnsAsync(new List<Reserva> {
                    new Reserva {
                        Id = 1,
                        NifUtilizador = "999999999",
                        ParqueId = 1,
                        Custo = 100,
                        TransacaoId = 1,
                        ReservaParqueId = 1,
                    },
                    new Reserva {
                        Id = 2,
                        NifUtilizador = "999999999",
                        ParqueId = 2,
                        Custo = 100,
                        TransacaoId = 1,
                        ReservaParqueId = 1,
                    }
                });

            ReservasController testController = new ReservasController(mock.Object);
            var result = await testController.GetReservaByNif("999999999");

            var items = Assert.IsType<List<Reserva>>(result.Value);
            Assert.Equal(2, items.Count);
            Assert.Equal("999999999", items[0].NifUtilizador);
            Assert.Equal("999999999", items[1].NifUtilizador);
            Assert.Equal(1, items[0].ParqueId);
            Assert.Equal(2, items[1].ParqueId);
        }


        [Fact]
        public async void GetReservaById_ShouldReturnFirstReserva()
        {
            //Arrange
            var mock = new Mock<IReservaService>();
            mock.Setup(p => p.GetByIdAsync(1))
                .ReturnsAsync(new Reserva
                {
                    Id = 1,
                    NifUtilizador = "999999999",
                    ParqueId = 1,
                    Custo = 100,
                    TransacaoId = 1,
                    ReservaParqueId = 1,
                });

            ReservasController testController = new ReservasController(mock.Object);

            var result = await testController.GetReservaById(1);

            var item = Assert.IsType<Reserva>(result.Value);
            Assert.Equal("999999999", item.NifUtilizador);
            Assert.Equal(1, item.ParqueId);
        }

        [Fact]
        public async void DeleteReservaById_ShouldDeleteFirstReserva()
        {
            //Arrange
            var mock = new Mock<IReservaService>();
            mock.Setup(p => p.GetByIdAsync(1))
                .ReturnsAsync(new Reserva
                {
                    Id = 1,
                    NifUtilizador = "999999999",
                    ParqueId = 1,
                    Custo = 100,
                    TransacaoId = 1,
                    ReservaParqueId = 1,
                });

            ReservasController testController = new ReservasController(mock.Object);

            var result = await testController.DeleteReserva(1);
            Assert.IsType<NoContentResult>(result);

        }

        [Fact]
        public async void PostReserva_ShouldPostOneReserva()
        {
            //Arrange
            var mock = new Mock<IReservaService>();
            mock.Setup(x => x.PostAsync(It.IsAny<ReservaDTO>())).ReturnsAsync(new Reserva
            {
                Id = 1,
                NifUtilizador = "999999999",
                ParqueId = 1,
                Custo = 200,
                TransacaoId = 1,
                ReservaParqueId = 1
            });

            var theNewReserva = new ReservaDTO
            {
                NifUtilizador = "999999999",
                NifVendedor = "111111111",
                ParqueId = 1,
                ApiUrl = "randomLink",
                MetodoId = 1,
                ReservaOriginalId = 1,
                Preco = 100,
                LugarId = 1,
                Inicio = DateTime.Parse("2021-01-30 11:00:00"),
                Fim = DateTime.Parse("2021-01-30 11:00:00")
            };


            ReservasController testController = new ReservasController(mock.Object);

            var response = await testController.PostReserva(theNewReserva);
            var result = GetObjectResultContent(response);

            Assert.NotNull(response);
            Assert.IsNotType<BadRequestObjectResult>(result);
            Assert.IsType<Reserva>(result);
            Assert.Equal("randomLink", theNewReserva.ApiUrl);
        }

        [Fact]
        public async void GetDisponilibidade_ShouldReturnLugaresDisponiveisNaFreguesiaDataHora()
        {
            //Arrange
            var mock = new Mock<IReservaService>();
            mock.Setup(p => p.FindAvailableAsync("Porto", DateTime.Parse("2021-01-30 11:00:00"), DateTime.Parse("2021-01-30 13:00:00")))
                .ReturnsAsync(new List<LugarDTO>
                {
                    new LugarDTO {
                        Id = 1,
                        ParqueId = 1,
                        Numero = 1,
                        Fila = "A",
                        Andar = 1,
                        Preco = 10,
                        NifProprietario = "999999999",
                        ReservaOriginalId = 1,
                        ParqueIdSC = 1,
                        ApiUrl = "randomlink"
                    }
                });

                ReservasController testController = new ReservasController(mock.Object);

                var result = await testController.FindAvailableAsync("Porto", DateTime.Parse("2021-01-30 11:00:00"), DateTime.Parse("2021-01-30 13:00:00"));

                var item = Assert.IsType<List<LugarDTO>>(result.Value);
                Assert.Equal(1, item[0].Id);
                Assert.Equal("A", item[0].Fila);
        }

        private static T GetObjectResultContent<T>(ActionResult<T> result)
        {
            return (T)((ObjectResult)result.Result).Value;
        }

    }
}