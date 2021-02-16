using API_Sistema_Central.Services;
using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using API_Sistema_Central.Models;
using API_Sistema_Central.Controllers;

namespace SCUnitTests
{
    /*public class UtilizadoresControllerTest
    {
            [Fact]
            public async void RegistarUtilizador_DeveRegistarUmUtilizador()
            {
                var mock = new Mock<IUtilizadorService>();
                mock.Setup(p => p.RegistarUtilizador())
                    .ReturnsAsync(new Utilizador {
                    new Utilizador {
                        Id = "999999999",
                        Nome = "Jakim das Coives",
                        //UserName = "Coives",
                        Carteira = 500,
                        CredencialId = 1
                    });
                UtilizadoresController testController = new UtilizadoresController(mock.Object);
                var result = await testController.RegistarUtilizador();
                var item = Assert.IsType<Utilizador>(result.Value);
                Assert.Equal("Jakim das Coives", item.Nome);
                Assert.Equal(500, item.Carteira);
            }
    }
}


            [Fact]
            public async void GetTransacaoById_ShouldReturnFirstTransacao()
            {
                //Arrange
                var mock = new Mock<ITransacaoService>();
                mock.Setup(p => p.Login(1))
                    .ReturnsAsync(new Transacao
                    {
                        Id = 1,
                        DataHora = DateTime.Now,
                        MetodoId = 1,
                        NifPagador = "999999999",
                        NifRecipiente = "111111111",
                        Valor = 123
                    });
                TransacoesController testController = new TransacoesController(mock.Object);

                //Act

                var result = await testController.GetTransacaoById(1);

                //Assert

                var item = Assert.IsType<Transacao>(result.Value);
                Assert.Equal("999999999", item.NifPagador);
                Assert.Equal("111111111", item.NifRecipiente);
            }*/
}
