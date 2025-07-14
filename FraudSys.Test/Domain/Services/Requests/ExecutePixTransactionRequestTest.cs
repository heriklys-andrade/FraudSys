using FluentAssertions;
using FraudSys.Domain.Services.Requests;

namespace FraudSys.Test.Domain.Services.Requests
{
    public class ExecutePixTransactionRequestTest
    {
        [Trait("Validate", "Success")]
        [Fact(DisplayName = "Valida com sucesso uma request criada")]
        public void ExecutePixTransactionRequest_Validate_Success()
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

            // Act & Assert
            request.Validate();
        }

        [Trait("Validate", "ThrowsException")]
        [Theory(DisplayName = "Levanta exceção ao validar request com dados inválidos")]
        [MemberData(nameof(InvalidExecutePixTransactionRequests), MemberType = typeof(ExecutePixTransactionRequestTest))]
        public void ExecutePixTransactionRequest_Validate_ThrowsExceptionWhenArgumentsAreInvalid(ExecutePixTransactionRequest request, string errorMessage)
        {
            // Act
            var result = Assert.Throws<ArgumentException>(request.Validate);

            //Assert
            result.Message.Should().Contain(errorMessage);
        }

        public static TheoryData<ExecutePixTransactionRequest, string> InvalidExecutePixTransactionRequests()
        {
            return new TheoryData<ExecutePixTransactionRequest, string>
            {
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = null,
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Documento do cliente de origem deve ser preenchido"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Documento do cliente de origem deve ser preenchido"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "  ",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Documento do cliente de origem deve ser preenchido"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "123",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Documento do cliente de origem deve conter 11 caracteres"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = null,
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Agência do cliente de origem deve ser preenchida"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Agência do cliente de origem deve ser preenchida"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "  ",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Agência do cliente de origem deve ser preenchida"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = null,
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Conta do cliente de origem deve ser preenchida"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = "",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Conta do cliente de origem deve ser preenchida"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = "  ",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Conta do cliente de origem deve ser preenchida"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = null,
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Documento do cliente de destino deve ser preenchido"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "",
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Documento do cliente de destino deve ser preenchido"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "  ",
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Documento do cliente de destino deve ser preenchido"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "123",
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Documento do cliente de destino deve conter 11 caracteres"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = null,
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Agência do cliente de destino deve ser preenchida"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Agência do cliente de destino deve ser preenchida"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "  ",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 100.00
                    },
                    "Agência do cliente de destino deve ser preenchida"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "202",
                        TargetClientAccount = null,
                        TransactionAmount = 100.00
                    },
                    "Conta do cliente de destino deve ser preenchida"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "202",
                        TargetClientAccount = "",
                        TransactionAmount = 100.00
                    },
                    "Conta do cliente de destino deve ser preenchida"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "202",
                        TargetClientAccount = "  ",
                        TransactionAmount = 100.00
                    },
                    "Conta do cliente de destino deve ser preenchida"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "12345678901",
                        TargetClientAgency = "101",
                        TargetClientAccount = "123-1",
                        TransactionAmount = 100.00
                    },
                    "Cliente de origem e destino não podem ser os mesmos"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 0
                    },
                    "Valor da transferência deve ser maior que zero"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = -500.00
                    },
                    "Valor da transferência deve ser maior que zero"
                },
                {
                    new ExecutePixTransactionRequest
                    {
                        SourceClientDocument = "12345678901",
                        SourceClientAgency = "101",
                        SourceClientAccount = "123-1",
                        TargetClientDocument = "10987654321",
                        TargetClientAgency = "202",
                        TargetClientAccount = "456-2",
                        TransactionAmount = 500.00326
                    },
                    "O valor do Limite Pix deve conter no máximo duas casas decimais"
                }
            };
        }
    }
}
