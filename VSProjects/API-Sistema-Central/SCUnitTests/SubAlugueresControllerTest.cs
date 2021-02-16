/*using System;
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
            mock.Setup(s => s.PostSubAluguerAsync)
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
            //Act
            var result = await controller.PostSubAluguer(model);
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
*/
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


           // mock.Setup(s => s.GetByIdAsync(1));


            // mock.Setup(s => s.DeleteSubAluguerAsync(It.Is<SubAluguerDTO>(s => Equals(s.Id, 1)))).Returns(Task.FromResult(true));

            //var mock = new Mock<ISubAluguerService>();



            // mock.Setup(x => x.UpdateAsync(It.Is<SubAluguerDTO>(c => string.Equals(c.CountryCode, "C1")))).Returns(Task.FromResult(true));
            // mock.Setup(x => x.UpdateAsync(It.Is<SubAluguerDTO>(c => string.Equals(c.CountryCode, "NoExCod")))).Returns(Task.FromResult(false));*/

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
    }
}
      /*  [Fact]
        public async Task GetSubAluguerById_ShouldReturnOneSubAluguer()
        {
            //Arrange        
            var theController = new SubAlugueresController(serviceMock);
            var SubAluguerId = 1;

            //Act
            var result = await theController.GetSubAluguerById(SubAluguerId);
            var SubAluguer = result.Value;

            //Assert     
            Assert.IsType<SubAluguerDTO>(SubAluguer);
            Assert.Equal(SubAluguer, (SubAluguer as SubAluguerDTO).Id);
        }
      */

        /*[Fact]
        public async Task GetSubAluguerById_ShouldReturnOneSubAluguer()
        {
            //Arrange        
            var theController = new CountryController(theMockedService);
            var testCod = 1;

            //Act
            var result = await theController.GetCountry(testCod);
            var countryItem = result.Value;

            //Assert     
            Assert.IsType<Country>(countryItem);
            Assert.Equal(testCod, (countryItem as Country).CountryCode);
        }
    }
}

     /*   [Fact]
        public async Task GetCountryAsync_ShouldReturnNotFound()
        {
            // Arrange      
            var theController = new CountryController(serviceMocker);
            var testCod = "C0";

            // Act
            var response = await theController.GetCountry(testCod);

            //Assert       
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public async Task GetSubAluguerByNifAsync_ShouldReturnAllSubAlugueresFrom1Nif()
        {
            // Arrange       
            var theController = new SubAlugueresController(serviceMocker);
            var testCod = "999999999";

            // Act
            var result = await theController.GetCountry(testCod);

            //Assert     
            Assert.IsType<Country>(result.Value);
        }

        [Fact]
        public async Task GetCountryAsync_ShouldReturnTheRightItemAsync()
        {
            //Arrange        
            var theController = new CountryController(theMockedService);
            var testCod = "C1";

            //Act
            var result = await theController.GetCountry(testCod);
            var countryItem = result.Value;

            //Assert     
            Assert.IsType<Country>(countryItem);
            Assert.Equal(testCod, (countryItem as Country).CountryCode);
        }

        [Fact]
        public async Task PostBadNoNameCountryAsync_ShouldReturnBadRequest()
        {
            // Arrange     
            var theController = new CountryController(theMockedService);
            var noNameCountry = new Country
            {
                CountryCode = "XYZ",
                GeoAreaCod = "Z1"
            };
            theController.ModelState.AddModelError("Name", "Required");

            // Act
            var response = await theController.PostCountry(noNameCountry);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response.Result);
        }

        [Fact]
        public async Task PostBadNoGeocodeCountryAsync_ShouldReturnBadRequest()
        {
            // Arrange   
            var theController = new CountryController(theMockedService);
            var noGeoAreaCountry = new Country
            {
                CountryCode = "XYZ",
                Name = "Bad country"
            };
            theController.ModelState.AddModelError("GeoAreaCod", "Required");

            // Act
            var response = await theController.PostCountry(noGeoAreaCountry);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response.Result);
        }

        [Fact]
        public async Task PostValidCountryAsync_ShouldReturnCreatedResponse()
        {
            // Arrange     
            var theController = new CountryController(theMockedService);
            var theNewCountry = new Country
            {
                CountryCode = "NEW",
                Name = "The New Country",
                GeoAreaCod = "Z1"
            };

            // Act
            var response = await theController.PostCountry(theNewCountry);

            // Assert
            Assert.IsType<CreatedAtActionResult>(response.Result);
        }

        [Fact]
        public async Task PostCountryAsync_ShouldCreateAnCountryAsync()
        {
            // Arrange       
            var theController = new CountryController(theMockedService);
            var theNewCountry = new Country
            {
                CountryCode = "OTHER",
                Name = "Another New Country",
                GeoAreaCod = "Z3"
            };

            // Act
            var response = await theController.PostCountry(theNewCountry);
            var result = response.Result as CreatedAtActionResult;

            // Assert
            Assert.NotNull(response);
            Assert.IsNotType<BadRequestObjectResult>(result);
            Assert.IsType<Country>(result.Value);

            var theCountry = result.Value as Country;
            Assert.Equal("OTHER", theCountry.CountryCode);
        }

        [Fact]
        public async Task PutNoExistingCountryAsync_ShouldReturnNotFound()
        {
            // Arrange      
            var theController = new CountryController(theMockedService);
            var testCod = "NoExCod";
            var theNonExistingCountry = new Country
            {
                CountryCode = testCod,
                Name = "Updated country " + testCod,
                GeoAreaCod = "Z1"
            };

            // Act
            var response = await theController.PutCountry(testCod, theNonExistingCountry);

            // Assert
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task PutBadNoNameCountryAsync_ShouldReturnBadRequest()
        {
            // Arrange      
            var theController = new CountryController(theMockedService);

            var testCod = "C1";
            var noNameCountry = new Country
            {
                CountryCode = testCod,
                GeoAreaCod = "Z1"
            };


            theController.ModelState.AddModelError("Name", "Required");

            // Act
            var response = await theController.PutCountry(testCod, noNameCountry);

            // Assert
            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public async Task PutBadNoGeoAreaCountryAsync_ShouldReturnBadRequest()
        {
            // Arrange        
            var theController = new CountryController(theMockedService);

            var testCod = "C1";
            var noNameCountry = new Country
            {
                CountryCode = testCod,
                Name = "Updated Name"
            };

            theController.ModelState.AddModelError("GeoAreaCod", "Required");

            // Act
            var response = await theController.PutCountry(testCod, noNameCountry);

            // Assert
            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public async Task PutCountry_ShouldReturnCreatedResponse()
        {
            // Arrange     
            var theController = new CountryController(theMockedService);
            var testCod = "C1";
            var theCountry = new Country
            {
                CountryCode = testCod,
                Name = "Updated Country",
                GeoAreaCod = "Z1"
            };

            // Act
            var response = await theController.PutCountry(testCod, theCountry);

            // Assert
            Assert.IsType<CreatedAtActionResult>(response);
        }

        [Fact]
        public async Task DeleteNotExistingCountry_ShouldReturnNotFound()
        {
            // Arrange       
            var theController = new CountryController(theMockedService);
            var testCod = "NoExCod";

            // Act
            var result = await theController.DeleteCountry(testCod);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteExistingCountryAsync_ShouldReturnOkResult()
        {
            // Arrange       
            var theController = new CountryController(theMockedService);
            var testCod = "C1";

            // Act
            var response = await theController.DeleteCountry(testCod);

            // Assert
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public async Task DeleteExistingCountryAsync_ShouldRemovetheCountryAsync()
        {
            // Arrange       
            var theController = new CountryController(theMockedService);
            var testCod = "C1";

            // Act
            var result = await theController.DeleteCountry(testCod);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
     */