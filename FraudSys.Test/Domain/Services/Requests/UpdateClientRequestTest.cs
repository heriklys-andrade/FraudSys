using FluentAssertions;
using FraudSys.Domain.Services.Requests;

namespace FraudSys.Test.Domain.Services.Requests
{
    public class UpdateClientRequestTest
    {
        [Trait("Validate", "Success")]
        [Fact(DisplayName = "Valida com sucesso uma request criada")]
        public void UpdateClientRequest_Validate_Success()
        {
            // Arrange
            var request = new UpdateClientRequest
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
        [MemberData(nameof(InvalidUpdateClientRequests), MemberType = typeof(UpdateClientRequestTest))]
        public void UpdateClientRequest_Validate_ThrowsExceptionWhenArgumentsAreInvalid(UpdateClientRequest request, string errorMessage)
        {
            // Act
            var result = Assert.Throws<ArgumentException>(request.Validate);

            //Assert
            result.Message.Should().Contain(errorMessage);
        }

        public static TheoryData<UpdateClientRequest, string> InvalidUpdateClientRequests()
        {
            return new TheoryData<UpdateClientRequest, string>
            {
                {
                    new UpdateClientRequest
                    {
                        ClientDocument = null,
                        ClientAgency = "101",
                        ClientAccount = "123-1",
                        ClientPixLimit = 1000.00
                    },
                    "Documento do cliente deve ser preenchido"
                },
                {
                    new UpdateClientRequest
                    {
                        ClientDocument = "",
                        ClientAgency = "101",
                        ClientAccount = "123-1",
                        ClientPixLimit = 1000.00
                    },
                    "Documento do cliente deve ser preenchido"
                },
                {
                    new UpdateClientRequest
                    {
                        ClientDocument = "  ",
                        ClientAgency = "101",
                        ClientAccount = "123-1",
                        ClientPixLimit = 1000.00
                    },
                    "Documento do cliente deve ser preenchido"
                },
                {
                    new UpdateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = null,
                        ClientAccount = "123-1",
                        ClientPixLimit = 1000.00
                    },
                    "Agência do cliente deve ser preenchida"
                },
                {
                    new UpdateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "",
                        ClientAccount = "123-1",
                        ClientPixLimit = 1000.00
                    },
                    "Agência do cliente deve ser preenchida"
                },
                {
                    new UpdateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "   ",
                        ClientAccount = "123-1",
                        ClientPixLimit = 1000.00
                    },
                    "Agência do cliente deve ser preenchida"
                },
                {
                    new UpdateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "101",
                        ClientAccount = null,
                        ClientPixLimit = 1000.00
                    },
                    "Conta do cliente deve ser preenchida"
                },
                {
                    new UpdateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "101",
                        ClientAccount = "",
                        ClientPixLimit = 1000.00
                    },
                    "Conta do cliente deve ser preenchida"
                },
                {
                    new UpdateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "101",
                        ClientAccount = "   ",
                        ClientPixLimit = 1000.00
                    },
                    "Conta do cliente deve ser preenchida"
                },
                {
                    new UpdateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "101",
                        ClientAccount = "123-1",
                        ClientPixLimit = 0
                    },
                    "Limite Pix do cliente deve ser maior que zero"
                },
                {
                    new UpdateClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "101",
                        ClientAccount = "123-1",
                        ClientPixLimit = -1000.00
                    },
                    "Limite Pix do cliente deve ser maior que zero"
                }
            };
        }
    }
}
