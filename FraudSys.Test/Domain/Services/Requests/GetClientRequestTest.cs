using FluentAssertions;
using FraudSys.Domain.Services.Requests;

namespace FraudSys.Test.Domain.Services.Requests
{
    public class GetClientRequestTest
    {
        [Trait("Validate", "Success")]
        [Fact(DisplayName = "Valida com sucesso uma request criada")]
        public void GetClientRequest_Validate_Success()
        {
            // Arrange
            var request = new GetClientRequest
            {
                ClientDocument = "12345678901",
                ClientAgency = "101",
                ClientAccount = "123-1"
            };

            // Act & Assert
            request.Validate();
        }

        [Trait("Validate", "ThrowsException")]
        [Theory(DisplayName = "Levanta exceção ao validar request com dados inválidos")]
        [MemberData(nameof(InvalidGetClientRequests), MemberType = typeof(GetClientRequestTest))]
        public void GetClientRequest_Validate_ThrowsExceptionWhenArgumentsAreInvalid(GetClientRequest request, string errorMessage)
        {
            // Act
            var result = Assert.Throws<ArgumentException>(request.Validate);

            //Assert
            result.Message.Should().Contain(errorMessage);
        }

        public static TheoryData<GetClientRequest, string> InvalidGetClientRequests()
        {
            return new TheoryData<GetClientRequest, string>
            {
                {
                    new GetClientRequest
                    {
                        ClientDocument = null,
                        ClientAgency = "101",
                        ClientAccount = "123-1"
                    },
                    "Documento do cliente deve ser preenchido"
                },
                {
                    new GetClientRequest
                    {
                        ClientDocument = "",
                        ClientAgency = "101",
                        ClientAccount = "123-1"
                    },
                    "Documento do cliente deve ser preenchido"
                },
                {
                    new GetClientRequest
                    {
                        ClientDocument = "  ",
                        ClientAgency = "101",
                        ClientAccount = "123-1"
                    },
                    "Documento do cliente deve ser preenchido"
                },
                {
                    new GetClientRequest
                    {
                        ClientDocument = "123",
                        ClientAgency = "101",
                        ClientAccount = "123-1"
                    },
                    "Documento do cliente deve conter 11 caracteres"
                },
                {
                    new GetClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = null,
                        ClientAccount = "123-1"
                    },
                    "Agência do cliente deve ser preenchida"
                },
                {
                    new GetClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "",
                        ClientAccount = "123-1"
                    },
                    "Agência do cliente deve ser preenchida"
                },
                {
                    new GetClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "   ",
                        ClientAccount = "123-1"
                    },
                    "Agência do cliente deve ser preenchida"
                },
                {
                    new GetClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "101",
                        ClientAccount = null
                    },
                    "Conta do cliente deve ser preenchida"
                },
                {
                    new GetClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "101",
                        ClientAccount = ""
                    },
                    "Conta do cliente deve ser preenchida"
                },
                {
                    new GetClientRequest
                    {
                        ClientDocument = "12345678901",
                        ClientAgency = "101",
                        ClientAccount = "   "
                    },
                    "Conta do cliente deve ser preenchida"
                }
            };
        }
    }
}
