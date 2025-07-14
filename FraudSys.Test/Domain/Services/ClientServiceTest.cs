using FluentAssertions;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces.Repositories;
using FraudSys.Domain.Services;
using FraudSys.Domain.Services.Requests;
using Moq;

namespace FraudSys.Test.Domain.Services
{
    public class ClientServiceTest
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock;

        private readonly ClientService service;

        public ClientServiceTest()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            service = new ClientService(_clientRepositoryMock.Object);
        }

        [Trait("CreateClientAsync", "Success")]
        [Fact(DisplayName = "Cria um cliente com sucesso")]
        public async Task CreateClientAsync_Success()
        {
            // Arrange
            var request = new CreateClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "101",
                ClientAccount = "123-1",
                ClientPixLimit = 1000.00
            };

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            // Act
            await service.CreateClientAsync(request, CancellationToken.None);

            // Assert
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.CreateClientAsync(It.IsAny<ClientEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Trait("CreateClientAsync", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar criar cliente já existente")]
        public async Task CreateClientAsync_ThrowsException_WhenClientAlreadyExists()
        {
            // Arrange
            var request = new CreateClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "101",
                ClientAccount = "123-1",
                ClientPixLimit = 1000.00
            };

            var existingClient = new ClientEntity(request.ClientDocument, request.ClientAgency, request.ClientAccount, request.ClientPixLimit);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingClient);

            // Act
            var result = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateClientAsync(request, CancellationToken.None));

            //Assert
            result.Message.Should().BeEquivalentTo("Cliente já cadastrado");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.CreateClientAsync(It.IsAny<ClientEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Trait("GetClientAsync", "Success")]
        [Fact(DisplayName = "Obtém os dados de um cliente com sucesso")]
        public async Task GetClientAsync_Success()
        {
            // Arrange
            var request = new GetClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "101",
                ClientAccount = "123-1"
            };

            var client = new ClientEntity(request.ClientDocument, request.ClientAgency, request.ClientAccount, 1000.00);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            // Act
            var result = await service.GetClientAsync(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new { ClientPixLimit = client.LimitePix });
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Trait("GetClientAsync", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar obter cliente inexistente")]
        public async Task GetClientAsync_ThrowsException_WhenClientNotFound()
        {
            // Arrange
            var request = new GetClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "101",
                ClientAccount = "123-1"
            };

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetClientAsync(request, CancellationToken.None));

            // Assert
            result.Message.Should().BeEquivalentTo("Cliente não encontrado");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Trait("GetClientAsync", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar obter cliente com dados da agência incorretos")]
        public async Task GetClientAsync_ThrowsException_WhenClientAgencyDoesNotMatch()
        {
            // Arrange
            var request = new GetClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "101",
                ClientAccount = "123-1"
            };
            var client = new ClientEntity(request.ClientDocument, "202", request.ClientAccount, 1000.00);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            // Act
            var result = await Assert.ThrowsAsync<ArgumentException>(() => service.GetClientAsync(request, CancellationToken.None));

            // Assert
            result.Message.Should().BeEquivalentTo("Agência do cliente não corresponde à agência solicitada");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Trait("GetClientAsync", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar obter cliente com dados da conta incorretos")]
        public async Task GetClientAsync_ThrowsException_WhenClientAccountDoesNotMatch()
        {
            // Arrange
            var request = new GetClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "101",
                ClientAccount = "123-1"
            };

            var client = new ClientEntity(request.ClientDocument, request.ClientAgency, "456-2", 1000.00);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            // Act
            var result = await Assert.ThrowsAsync<ArgumentException>(() => service.GetClientAsync(request, CancellationToken.None));

            // Assert
            result.Message.Should().BeEquivalentTo("Conta do cliente não corresponde à conta solicitada");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Trait("UpdateClientAsync", "Success")]
        [Fact(DisplayName = "Atualiza os dados de um cliente com sucesso")]
        public async Task UpdateClientAsync_Success()
        {
            // Arrange
            var request = new UpdateClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "101",
                ClientAccount = "123-1",
                ClientPixLimit = 2000.00
            };

            var client = new ClientEntity(request.ClientDocument, request.ClientAgency, request.ClientAccount, 1000.00);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            // Act
            var result = await service.UpdateClientAsync(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new { ClientPixLimit = 2000.00 });
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.UpdateClientAsync(It.Is<ClientEntity>(c => c.LimitePix == 2000.00), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Trait("UpdateClientAsync", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar atualizar cliente inexistente")]
        public async Task UpdateClientAsync_ThrowsException_WhenClientNotFound()
        {
            // Arrange
            var request = new UpdateClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "101",
                ClientAccount = "123-1",
                ClientPixLimit = 2000.00
            };

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateClientAsync(request, CancellationToken.None));

            // Assert
            result.Message.Should().BeEquivalentTo("Cliente não encontrado");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.UpdateClientAsync(It.IsAny<ClientEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Trait("UpdateClientAsync", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar atualizar cliente com dados da agência incorretos")]
        public async Task UpdateClientAsync_ThrowsException_WhenClientAgencyDoesNotMatch()
        {
            // Arrange
            var request = new UpdateClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "101",
                ClientAccount = "123-1",
                ClientPixLimit = 2000.00
            };

            var client = new ClientEntity(request.ClientDocument, "202", request.ClientAccount, 1000.00);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            // Act
            var result = await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateClientAsync(request, CancellationToken.None));

            // Assert
            result.Message.Should().BeEquivalentTo("Agência do cliente não corresponde à agência solicitada");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.UpdateClientAsync(It.IsAny<ClientEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Trait("UpdateClientAsync", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar atualizar cliente com dados da conta incorretos")]
        public async Task UpdateClientAsync_ThrowsException_WhenClientAccountDoesNotMatch()
        {
            // Arrange
            var request = new UpdateClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "101",
                ClientAccount = "123-1",
                ClientPixLimit = 2000.00
            };

            var client = new ClientEntity(request.ClientDocument, request.ClientAgency, "456-2", 1000.00);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            // Act
            var result = await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateClientAsync(request, CancellationToken.None));

            // Assert
            result.Message.Should().BeEquivalentTo("Conta do cliente não corresponde à conta solicitada");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.UpdateClientAsync(It.IsAny<ClientEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Trait("DeleteClientAsync", "Success")]
        [Fact(DisplayName = "Exclui um cliente com sucesso")]
        public async Task DeleteClientAsync_Success()
        {
            // Arrange
            var request = new DeleteClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "101",
                ClientAccount = "123-1"
            };

            var client = new ClientEntity(request.ClientDocument, request.ClientAgency, request.ClientAccount, 1000.00);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            // Act
            await service.DeleteClientAsync(request, CancellationToken.None);

            // Assert
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.DeleteClientAsync(client, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Trait("DeleteClientAsync", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar excluir cliente inexistente")]
        public async Task DeleteClientAsync_ThrowsException_WhenClientNotFound()
        {
            // Arrange
            var request = new DeleteClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "101",
                ClientAccount = "123-1"
            };

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteClientAsync(request, CancellationToken.None));

            // Assert
            result.Message.Should().BeEquivalentTo("Cliente não encontrado");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.DeleteClientAsync(It.IsAny<ClientEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Trait("DeleteClientAsync", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar excluir cliente com dados da agência incorretos")]
        public async Task DeleteClientAsync_ThrowsException_WhenClientAgencyDoesNotMatch()
        {
            // Arrange
            var request = new DeleteClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "101",
                ClientAccount = "123-1"
            };

            var client = new ClientEntity(request.ClientDocument, "202", request.ClientAccount, 1000.00);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            // Act
            var result = await Assert.ThrowsAsync<ArgumentException>(() => service.DeleteClientAsync(request, CancellationToken.None));

            // Assert
            result.Message.Should().BeEquivalentTo("Agência do cliente não corresponde à agência solicitada");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.DeleteClientAsync(It.IsAny<ClientEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Trait("DeleteClientAsync", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar excluir cliente com dados da conta incorretos")]
        public async Task DeleteClientAsync_ThrowsException_WhenClientAccountDoesNotMatch()
        {
            // Arrange
            var request = new DeleteClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "101",
                ClientAccount = "123-1"
            };

            var client = new ClientEntity(request.ClientDocument, request.ClientAgency, "456-2", 1000.00);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            // Act
            var result = await Assert.ThrowsAsync<ArgumentException>(() => service.DeleteClientAsync(request, CancellationToken.None));

            // Assert
            result.Message.Should().BeEquivalentTo("Conta do cliente não corresponde à conta solicitada");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.ClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.DeleteClientAsync(It.IsAny<ClientEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}