using API_Sistema_Central.Services;
using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using API_Sistema_Central.Models;
using API_Sistema_Central.Controllers;
using API_Sistema_Central.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SCUnitTests
{
   /* public class UtilizadoresControllerTest {
        [Fact]
        public async Task PostRegistoUtilizador_ShouldPostOneRegisto()
        {
            //Arrange
            var mock = new Mock<IUtilizadorService>();
            mock.Setup(x => x.RegistarUtilizador(It.IsAny<RegistarUtilizadorDTO>())).ReturnsAsync(new Utilizador
            {
                Id = "9",
                Nome = "Jakim das Coives",
                Carteira = 500,
                CredencialId = 1
            });

            var theNewRegisto = new RegistarUtilizadorDTO
            {
                Nif = "999999999",
                NomeUtilizador = "Jakim das Coives",
                EmailUtilizador = "test@test.com",
                PasswordUtilizador = "123Pa$$word",
                MetodoId = 3,
                EmailPayPal = "test@test.com",
                PasswordPayPal = "123Pa$$word"
            }; 
            UtilizadoresController testController = new UtilizadoresController(mock.Object);

            var response = await testController.RegistarUtilizador(theNewRegisto);
            var result = GetObjectResultContent(response);

            Assert.NotNull(response);
            Assert.IsNotType<BadRequestObjectResult>(result);
            Assert.IsType<Reserva>(result);
            Assert.Equal("test@test.com", theNewRegisto.EmailPayPal);
        }
        private static T GetObjectResultContent<T>(ActionResult<T> result)
        {
            return (T)((ObjectResult)result.Result).Value;
        }
    }*/
}

