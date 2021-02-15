using API_Sistema_Central.Services;
using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using API_Sistema_Central.Models;
using API_Sistema_Central.Controllers;

namespace SCUnitTests
{
    public class TransacoesControllerTest
    {
        [Fact]
        public async void GetAllTransacoesByNif_ShouldReturnMultipleTransacoesFrom999999999()
        {
            var mock = new Mock<ITransacaoService>();
            mock.Setup(p => p.GetByNifAsync("999999999"))
                .ReturnsAsync(new List<Transacao> {
                    new Transacao {
                        Id = 1,
                        DataHora = DateTime.Now,
                        MetodoId = 1,
                        NifPagador = "999999999",
                        NifRecipiente = "111111111",
                        Valor = 123
                    },
                    new Transacao {
                        Id = 2,
                        DataHora = DateTime.Now,
                        MetodoId = 2,
                        NifPagador = "222222222",
                        NifRecipiente = "999999999",
                        Valor = 321
                    }
                });
            TransacoesController testController = new TransacoesController(mock.Object);
            var result = await testController.GetTransacaoByNif("999999999");
            var items = Assert.IsType<List<Transacao>>(result.Value);
            Assert.Equal(2, items.Count);
            Assert.Equal("999999999", items[0].NifPagador);
            Assert.Equal("999999999", items[1].NifRecipiente);
        }


        [Fact]
        public async void GetTransacaoById_ShouldReturnFirstTransacao()
        {
            //Arrange
            var mock = new Mock<ITransacaoService>();
            mock.Setup(p => p.GetByIdAsync(1))
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
        }

    }
}
