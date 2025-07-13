using FluentAssertions;
using FraudSys.Domain.Entities;
using FraudSys.Domain.Interfaces.Repositories;
using FraudSys.Domain.Services;
using FraudSys.Domain.Services.Requests;
using Moq;
using System.Linq.Expressions;

namespace FraudSys.Test.Domain.Services
{
    public class TransactionServiceTest
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock;

        private readonly TransactionService service;

        public TransactionServiceTest()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            service = new TransactionService(_clientRepositoryMock.Object);
        }

        [Trait("ExecutePixTransaction", "Success")]
        [Fact(DisplayName = "Realiza a transferência pix entre contas com sucesso")]
        public async Task ExecutePixTransaction_Success()
        {
            // Arrange
            var request = new ExecutePixTransactionRequest
            {
                SourceClientDocument = "12345678901",
                SourceClientAgency = "101",
                SourceClientAccount = "123-1",
                TargetClientDocument = "10987654321",
                TargetClientAgency = "202",
                TargetClientAccount = "456-2",
                TransactionAmount = 100.00
            };

            var sourceClient = new ClientEntity("12345678901", "101", "123-1", 1000.00);
            var targetClient = new ClientEntity("10987654321", "202", "456-2", 500.00);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.SourceClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sourceClient);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.TargetClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(targetClient);

            // Act
            var result = await service.ExecutePixTransaction(request, CancellationToken.None);

            //Assert
            result.Should().BeEquivalentTo(new { ClientPixLimit = 900.00 });
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.SourceClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.TargetClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.UpdateClientsAsync(It.Is(GetPredicate(request)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Trait("ExecutePixTransaction", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar realizar transferência pix com limite insuficiente")]
        public async Task ExecutePixTransaction_ThrowsException_WhenInsufficientPixLimit()
        {
            // Arrange
            var request = new ExecutePixTransactionRequest
            {
                SourceClientDocument = "12345678901",
                SourceClientAgency = "101",
                SourceClientAccount = "123-1",
                TargetClientDocument = "10987654321",
                TargetClientAgency = "202",
                TargetClientAccount = "456-2",
                TransactionAmount = 1100.00
            };

            var sourceClient = new ClientEntity("12345678901", "101", "123-1", 1000.00);
            var targetClient = new ClientEntity("10987654321", "202", "456-2", 500.00);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.SourceClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sourceClient);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.TargetClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(targetClient);

            // Act
            var result = await Assert.ThrowsAsync<InvalidOperationException>(() => service.ExecutePixTransaction(request, CancellationToken.None));

            //Assert
            result.Message.Should().BeEquivalentTo("Limite Pix insuficiente para a transação");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.SourceClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.TargetClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.UpdateClientsAsync(It.IsAny<IEnumerable<ClientEntity>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Trait("ExecutePixTransaction", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar realizar transferência pix com cliente de origem não encontrado")]
        public async Task ExecutePixTransaction_ThrowsException_WhenSourceClientNotFound()
        {
            // Arrange
            var request = new ExecutePixTransactionRequest
            {
                SourceClientDocument = "12345678901",
                SourceClientAgency = "101",
                SourceClientAccount = "123-1",
                TargetClientDocument = "10987654321",
                TargetClientAgency = "202",
                TargetClientAccount = "456-2",
                TransactionAmount = 100.00
            };

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.SourceClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await Assert.ThrowsAsync<InvalidOperationException>(() => service.ExecutePixTransaction(request, CancellationToken.None));

            //Assert
            result.Message.Should().BeEquivalentTo("Cliente de origem não encontrado");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.SourceClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.TargetClientDocument, It.IsAny<CancellationToken>()), Times.Never);
            _clientRepositoryMock.Verify(repo => repo.UpdateClientsAsync(It.IsAny<IEnumerable<ClientEntity>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Trait("ExecutePixTransaction", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar realizar transferência pix com cliente de destino não encontrado")]
        public async Task ExecutePixTransaction_ThrowsException_WhenTargetClientNotFound()
        {
            // Arrange
            var request = new ExecutePixTransactionRequest
            {
                SourceClientDocument = "12345678901",
                SourceClientAgency = "101",
                SourceClientAccount = "123-1",
                TargetClientDocument = "10987654321",
                TargetClientAgency = "202",
                TargetClientAccount = "456-2",
                TransactionAmount = 100.00
            };

            var sourceClient = new ClientEntity("12345678901", "101", "123-1", 1000.00);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.SourceClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sourceClient);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.TargetClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await Assert.ThrowsAsync<InvalidOperationException>(() => service.ExecutePixTransaction(request, CancellationToken.None));

            //Assert
            result.Message.Should().BeEquivalentTo("Cliente de destino não encontrado");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.SourceClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.TargetClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.UpdateClientsAsync(It.IsAny<IEnumerable<ClientEntity>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Trait("ExecutePixTransaction", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar realizar transferência pix referenciando uma agência incorreta do cliente de origem")]
        public async Task ExecutePixTransaction_ThrowsException_WhenSourceClientAgencyDoesNotMatch()
        {
            // Arrange
            var request = new ExecutePixTransactionRequest
            {
                SourceClientDocument = "12345678901",
                SourceClientAgency = "101",
                SourceClientAccount = "123-1",
                TargetClientDocument = "10987654321",
                TargetClientAgency = "202",
                TargetClientAccount = "456-2",
                TransactionAmount = 100.00
            };
            var sourceClient = new ClientEntity("12345678901", "102", "123-1", 1000.00);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.SourceClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sourceClient);

            // Act
            var result = await Assert.ThrowsAsync<ArgumentException>(() => service.ExecutePixTransaction(request, CancellationToken.None));

            //Assert
            result.Message.Should().BeEquivalentTo("Agência do cliente de origem não corresponde à agência solicitada");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.SourceClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.TargetClientDocument, It.IsAny<CancellationToken>()), Times.Never);
            _clientRepositoryMock.Verify(repo => repo.UpdateClientsAsync(It.IsAny<IEnumerable<ClientEntity>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Trait("ExecutePixTransaction", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar realizar transferência pix referenciando uma conta incorreta do cliente de origem")]
        public async Task ExecutePixTransaction_ThrowsException_WhenSourceClientAccountDoesNotMatch()
        {
            // Arrange
            var request = new ExecutePixTransactionRequest
            {
                SourceClientDocument = "12345678901",
                SourceClientAgency = "101",
                SourceClientAccount = "123-1",
                TargetClientDocument = "10987654321",
                TargetClientAgency = "202",
                TargetClientAccount = "456-2",
                TransactionAmount = 100.00
            };

            var sourceClient = new ClientEntity("12345678901", "101", "124-1", 1000.00);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.SourceClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sourceClient);

            // Act
            var result = await Assert.ThrowsAsync<ArgumentException>(() => service.ExecutePixTransaction(request, CancellationToken.None));

            //Assert
            result.Message.Should().BeEquivalentTo("Conta do cliente de origem não corresponde à conta solicitada");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.SourceClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.TargetClientDocument, It.IsAny<CancellationToken>()), Times.Never);
            _clientRepositoryMock.Verify(repo => repo.UpdateClientsAsync(It.IsAny<IEnumerable<ClientEntity>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Trait("ExecutePixTransaction", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar realizar transferência pix referenciando uma agência incorreta do cliente de destino")]
        public async Task ExecutePixTransaction_ThrowsException_WhenTargetClientAgencyDoesNotMatch()
        {
            // Arrange
            var request = new ExecutePixTransactionRequest
            {
                SourceClientDocument = "12345678901",
                SourceClientAgency = "101",
                SourceClientAccount = "123-1",
                TargetClientDocument = "10987654321",
                TargetClientAgency = "202",
                TargetClientAccount = "456-2",
                TransactionAmount = 100.00
            };

            var sourceClient = new ClientEntity("12345678901", "101", "123-1", 1000.00);
            var targetClient = new ClientEntity("10987654321", "203", "456-2", 500.00);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.SourceClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sourceClient);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.TargetClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(targetClient);

            // Act
            var result = await Assert.ThrowsAsync<ArgumentException>(() => service.ExecutePixTransaction(request, CancellationToken.None));

            //Assert
            result.Message.Should().BeEquivalentTo("Agência do cliente de destino não corresponde à agência solicitada");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.SourceClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.TargetClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.UpdateClientsAsync(It.IsAny<IEnumerable<ClientEntity>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Trait("ExecutePixTransaction", "ThrowsException")]
        [Fact(DisplayName = "Lança exceção ao tentar realizar transferência pix referenciando uma conta incorreta do cliente de destino")]
        public async Task ExecutePixTransaction_ThrowsException_WhenTargetClientAccountDoesNotMatch()
        {
            // Arrange
            var request = new ExecutePixTransactionRequest
            {
                SourceClientDocument = "12345678901",
                SourceClientAgency = "101",
                SourceClientAccount = "123-1",
                TargetClientDocument = "10987654321",
                TargetClientAgency = "202",
                TargetClientAccount = "456-2",
                TransactionAmount = 100.00
            };

            var sourceClient = new ClientEntity("12345678901", "101", "123-1", 1000.00);
            var targetClient = new ClientEntity("10987654321", "202", "457-2", 500.00);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.SourceClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(sourceClient);

            _clientRepositoryMock.Setup(repo => repo.GetClientByPkAsync(request.TargetClientDocument, It.IsAny<CancellationToken>()))
                .ReturnsAsync(targetClient);

            // Act
            var result = await Assert.ThrowsAsync<ArgumentException>(() => service.ExecutePixTransaction(request, CancellationToken.None));

            //Assert
            result.Message.Should().BeEquivalentTo("Conta do cliente de destino não corresponde à conta solicitada");
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.SourceClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.GetClientByPkAsync(request.TargetClientDocument, It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.Verify(repo => repo.UpdateClientsAsync(It.IsAny<IEnumerable<ClientEntity>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private static Expression<Func<IEnumerable<ClientEntity>, bool>> GetPredicate(ExecutePixTransactionRequest request)
        {
            return x =>
                x.Any(c => c.Documento == request.SourceClientDocument && c.Agencia == request.SourceClientAgency && c.Conta == request.SourceClientAccount && c.LimitePix == 900.00) &&
                x.Any(c => c.Documento == request.TargetClientDocument && c.Agencia == request.TargetClientAgency && c.Conta == request.TargetClientAccount && c.LimitePix == 600.00);
        }
    }
}
