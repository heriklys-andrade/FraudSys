using FluentAssertions;
using FraudSys.Domain.Services.Requests;

namespace FraudSys.Test.Domain.Services.Requests
{
    public class CreateClientRequestTest
    {
        [Trait("Validate", "Success")]
        [Fact(DisplayName = "Valida com sucesso uma request criada")]
        public void CreateClientRequest_Validate_Success()
        {
            // Arrange
            var request = new CreateClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "101",
                ClientAccount = "123-1",
                ClientPixLimit = 1000.00
            };

            // Act & Assert
            request.Validate();
        }

        [Trait("Validate", "ThrowsException")]
        [Theory(DisplayName = "Levanta exceção ao validar request com dados inválidos")]
        [MemberData(nameof(InvalidCreateClientRequests), MemberType = typeof(CreateClientRequestTest))]
        public void CreateClientRequest_Validate_ThrowsExceptionWhenArgumentsAreInvalid(CreateClientRequest request, string errorMessage)
        {
            // Act
            var result = Assert.Throws<ArgumentException>(request.Validate);

            //Assert
            result.Message.Should().Contain(errorMessage);
        }

        public static TheoryData<CreateClientRequest, string> InvalidCreateClientRequests()
        {
            return new TheoryData<CreateClientRequest, string>
            {
                {
                    new CreateClientRequest
                    {
                        ClientDocument = null,
                        ClientAgency = "101",
                        ClientAccount = "123-1",
                        ClientPixLimit = 1000.00
                    },
                    "Documento do cliente deve ser preenchido"
                },
                {
                    new CreateClientRequest
                    {
                        ClientDocument = "",
                        ClientAgency = "101",
                        ClientAccount = "123-1",
                        ClientPixLimit = 1000.00
                    },
                    "Documento do cliente deve ser preenchido"
                },
                {
                    new CreateClientRequest
                    {
                        ClientDocument = "  ",
                        ClientAgency = "101",
                        ClientAccount = "123-1",
                        ClientPixLimit = 1000.00
                    },
                    "Documento do cliente deve ser preenchido"
                },
                {
                    new CreateClientRequest
                    {
                        ClientDocument = "123",
                        ClientAgency = "101",
                        ClientAccount = "123-1",
                        ClientPixLimit = 1000.00
                    },
                    "Documento do cliente deve conter 11 caracteres"
                },
                {
                    new CreateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = null,
                        ClientAccount = "123-1",
                        ClientPixLimit = 1000.00
                    },
                    "Agência do cliente deve ser preenchida"
                },
                {
                    new CreateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "",
                        ClientAccount = "123-1",
                        ClientPixLimit = 1000.00
                    },
                    "Agência do cliente deve ser preenchida"
                },
                {
                    new CreateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "   ",
                        ClientAccount = "123-1",
                        ClientPixLimit = 1000.00
                    },
                    "Agência do cliente deve ser preenchida"
                },
                {
                    new CreateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "101",
                        ClientAccount = null,
                        ClientPixLimit = 1000.00
                    },
                    "Conta do cliente deve ser preenchida"
                },
                {
                    new CreateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "101",
                        ClientAccount = "",
                        ClientPixLimit = 1000.00
                    },
                    "Conta do cliente deve ser preenchida"
                },
                {
                    new CreateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "101",
                        ClientAccount = "   ",
                        ClientPixLimit = 1000.00
                    },
                    "Conta do cliente deve ser preenchida"
                },
                {
                    new CreateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "101",
                        ClientAccount = "123-1",
                        ClientPixLimit = 0
                    },
                    "Limite Pix do cliente deve ser maior que zero"
                },
                {
                    new CreateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "101",
                        ClientAccount = "123-1",
                        ClientPixLimit = -1000.00
                    },
                    "Limite Pix do cliente deve ser maior que zero"
                },
                {
                    new CreateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "101",
                        ClientAccount = "123-1",
                        ClientPixLimit = 1000.0012
                    },
                    "O valor do Limite Pix deve conter no máximo duas casas decimais"
                }
            };
        }
    }
}
