using API_Sistema_Central.Services;
using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using API_Sistema_Central.Models;
using API_Sistema_Central.Controllers;
using API_Sistema_Central.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace SCUnitTests
{
   public class UtilizadoresControllerTest {
        [Fact]
        public async Task RegistoUtilizador_ShouldReturnAToken()
        {
            //Arrange
            var mock = new Mock<IUtilizadorService>();
            mock.Setup(x => x.RegistarUtilizador(It.IsAny<RegistarUtilizadorDTO>())).ReturnsAsync(IdentityResult.Success);

            var tokenMock = new Mock<ITokenService>();
            tokenMock.Setup(x => x.BuildToken(It.IsAny<InfoUtilizadorDTO>())).Returns(new TokenUtilizadorDTO { Token = "TestToken", Expiration = DateTime.Now});

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
            UtilizadoresController testController = new UtilizadoresController(tokenMock.Object, mock.Object);

            var response = await testController.RegistarUtilizador(theNewRegisto);

            var item = Assert.IsType<TokenUtilizadorDTO>(response.Value);
            Assert.NotNull(response);
            Assert.NotNull(item);
            Assert.Equal("TestToken", item.Token);
        }

        [Fact]
        public async Task LoginUtilizador_ShouldReturnAToken()
        {
            //Arrange
            var mock = new Mock<IUtilizadorService>();
            mock.Setup(x => x.Login(It.IsAny<InfoUtilizadorDTO>())).ReturnsAsync(SignInResult.Success);

            var tokenMock = new Mock<ITokenService>();
            tokenMock.Setup(x => x.BuildToken(It.IsAny<InfoUtilizadorDTO>())).Returns(new TokenUtilizadorDTO { Token = "TestToken", Expiration = DateTime.Now });

            var theNewRegisto = new InfoUtilizadorDTO { Email = "jakimdascoives@test.pt", Password = "coivinhas123"};

            UtilizadoresController testController = new UtilizadoresController(tokenMock.Object, mock.Object);

            var response = await testController.Login(theNewRegisto);

            var item = Assert.IsType<TokenUtilizadorDTO>(response.Value);
            Assert.NotNull(response);
            Assert.NotNull(item);
            Assert.Equal("TestToken", item.Token);
        }
    }
}

