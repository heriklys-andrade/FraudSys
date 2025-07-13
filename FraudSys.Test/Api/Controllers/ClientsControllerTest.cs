using FluentAssertions;
using FraudSys.Api.Controllers;
using FraudSys.Domain.Interfaces.Services;
using FraudSys.Domain.Services.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FraudSys.Test.Api.Controllers
{
    public class ClientsControllerTest
    {
        private readonly Mock<IClientService> _clientServiceMock;

        private readonly ClientsController controller;

        public ClientsControllerTest()
        {
            _clientServiceMock = new Mock<IClientService>();
            controller = new ClientsController(_clientServiceMock.Object);
        }

        [Trait("CreateClient", "Success")]
        [Fact(DisplayName = "Cria um cliente com sucesso")]
        public async Task CreateClient_ShouldCallServiceAndReturnCreated()
        {
            // Arrange
            var request = new CreateClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "1234",
                ClientAccount = "56789-0",
                ClientPixLimit = 1000.00
            };

            _clientServiceMock.Setup(s => s.CreateClientAsync(request, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await controller.CreateClient(request, CancellationToken.None);

            // Assert
            Assert.IsType<CreatedResult>(result);
            _clientServiceMock.Verify(s => s.CreateClientAsync(request, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Trait("GetClient", "Success")]
        [Fact(DisplayName = "Obtém um cliente com sucesso")]
        public async Task GetClient_ShouldCallServiceAndReturnOk()
        {
            // Arrange
            var request = new GetClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "1234",
                ClientAccount = "56789-0"
            };

            var expectedClient = new { ClientDocument = "12345678901", ClientAgency = "1234", ClientAccount = "56789-0", ClientPixLimit = 1000.00 };

            _clientServiceMock.Setup(s => s.GetClientAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedClient);

            // Act
            var result = await controller.GetClient(request, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(expectedClient);
            _clientServiceMock.Verify(s => s.GetClientAsync(request, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Trait("UpdateClient", "Success")]
        [Fact(DisplayName = "Atualiza um cliente com sucesso")]
        public async Task UpdateClient_ShouldCallServiceAndReturnOk()
        {
            // Arrange
            var request = new UpdateClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "1234",
                ClientAccount = "56789-0",
                ClientPixLimit = 1500.00
            };

            _clientServiceMock.Setup(s => s.UpdateClientAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new { ClientPixLimit = 1500.00 });

            // Act
            var result = await controller.UpdateClient(request, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(new { ClientPixLimit = 1500.00 });
            _clientServiceMock.Verify(s => s.UpdateClientAsync(request, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Trait("DeleteClient", "Success")]
        [Fact(DisplayName = "Exclui um cliente com sucesso")]
        public async Task DeleteClient_ShouldCallServiceAndReturnNoContent()
        {
            // Arrange
            var request = new DeleteClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "1234",
                ClientAccount = "56789-0"
            };

            _clientServiceMock.Setup(s => s.DeleteClientAsync(request, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await controller.DeleteClient(request, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _clientServiceMock.Verify(s => s.DeleteClientAsync(request, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
