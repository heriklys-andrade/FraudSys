using Bogus;
using FluentAssertions;
using FraudSys.Domain.Entities;

namespace FraudSys.Test.Domain.Entities
{
    public class ClientEntityTest
    {
        [Trait("Constructor", "Success")]
        [Fact(DisplayName = "Instancia uma entidade de cliente com sucesso")]
        public void ClientEntityConstructor_Success()
        {
            //Arrange
            string documento = "12345678901";
            string agencia = "101";
            string conta = "123-1";
            double limitePix = 1000.00;
            var expectedClient = new Faker<ClientEntity>()
                .RuleFor(c => c.Documento, f => documento)
                .RuleFor(c => c.Agencia, f => agencia)
                .RuleFor(c => c.Conta, f => conta)
                .RuleFor(c => c.LimitePix, f => limitePix)
                .Generate();

            //Act
            var client = new ClientEntity(documento, agencia, conta, limitePix);

            //Assert
            client.Should().BeEquivalentTo(expectedClient);
        }

        [Trait("UpdatePixLimit", "Success")]
        [Fact(DisplayName = "Altera o limite pix do cliente com sucesso")]
        public void UpdatePixLimit_Success()
        {
            // Arrange
            string documento = "12345678901";
            string agencia = "101";
            string conta = "123-1";
            double limitePix = 1000.00;
            var client = new ClientEntity(documento, agencia, conta, limitePix);
            double novoLimite = 2000.00;
            
            // Act
            client.UpdatePixLimit(novoLimite);
            
            // Assert
            client.LimitePix.Should().Be(novoLimite);
        }

        [Trait("UpdatePixLimit", "Success")]
        [Theory(DisplayName = "Levanta exceção ao tentar alterar o limite pix do cliente com valores inválidos")]
        [InlineData(0)]
        [InlineData(-2000)]
        [InlineData(-15.0)]
        public void UpdatePixLimit_ThrowsArgumentException_WhenNewLimitIsZeroOrNegative(double novoLimite)
        {
            // Arrange
            string documento = "12345678901";
            string agencia = "101";
            string conta = "123-1";
            double limitePix = 1000.00;
            var client = new ClientEntity(documento, agencia, conta, limitePix);

            // Act
            Action act = () => client.UpdatePixLimit(novoLimite);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Limite Pix deve ser maior que zero*")
                .WithParameterName("novoLimite");
        }
    }
}
