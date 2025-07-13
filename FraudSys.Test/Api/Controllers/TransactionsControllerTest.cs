using FluentAssertions;
using FraudSys.Api.Controllers;
using FraudSys.Domain.Interfaces.Services;
using FraudSys.Domain.Services.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FraudSys.Test.Api.Controllers
{
    public class TransactionsControllerTest
    {
        private readonly Mock<ITransactionService> _transactionServiceMock;

        private readonly TransactionsController controller;

        public TransactionsControllerTest()
        {
            _transactionServiceMock = new Mock<ITransactionService>();
            controller = new TransactionsController(_transactionServiceMock.Object);
        }

        [Trait("ExecuteTransaction", "Success")]
        [Fact(DisplayName = "Executa uma transação PIX com sucesso")]
        public async Task ExecuteTransaction_Success()
        {
            // Arrange
            var request = new ExecutePixTransactionRequest
            {
                SourceClientDocument = "12345678901",
                SourceClientAgency = "1234",
                SourceClientAccount = "56789-0",
                TargetClientDocument = "10987654321",
                TargetClientAgency = "4321",
                TargetClientAccount = "09876-5",
                TransactionAmount = 100.00
            };

            _transactionServiceMock.Setup(s => s.ExecutePixTransaction(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new { ClientPixLimit = 900.00 });

            // Act
            var result = await controller.ExecuteTransaction(request, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(new { ClientPixLimit = 900.00 });
            _transactionServiceMock.Verify(s => s.ExecutePixTransaction(request, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
