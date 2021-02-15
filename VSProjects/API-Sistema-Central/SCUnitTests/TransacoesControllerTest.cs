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
        public void GetAllTransacoesByNif_ShouldReturnMultipleTransacoesFrom999999999()
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
                    },
                });
            TransacoesController testController = new TransacoesController(mock.Object);
            var result = testController.GetTransacaoByNif("999999999");
        }
    }
}
