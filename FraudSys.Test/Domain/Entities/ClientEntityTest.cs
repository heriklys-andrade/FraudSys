using Bogus;
using FluentAssertions;
using FraudSys.Domain.Entities;

namespace FraudSys.Test.Domain.Entities
{
    public class ClientEntityTest
    {
        [Fact]
        public void ClientEntityConstructor_Success()
        {
            //Arrange
            string documento = "12345678901";
            int agencia = 101;
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
    }
}
