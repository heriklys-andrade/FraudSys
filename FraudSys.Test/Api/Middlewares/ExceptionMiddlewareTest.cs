using FluentAssertions;
using FraudSys.Api.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace FraudSys.Test.Api.Middlewares
{
    public class ExceptionMiddlewareTest
    {
        private readonly Mock<ILogger<ExceptionMiddleware>> _loggerMock;

        public ExceptionMiddlewareTest()
        {
            _loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
        }

        [Trait("InvokeAsync", "Success")]
        [Theory(DisplayName = "Executa com sucesso uma requisição HTTP")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task InvokeAsync_Success(bool isDevelopment)
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            RequestDelegate next = async (ctx) =>
            {
                ctx.Response.StatusCode = StatusCodes.Status200OK;
                await ctx.Response.WriteAsync("Sucesso");
            };

            var middleware = new ExceptionMiddleware(next, _loggerMock.Object, isDevelopment);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Trait("InvokeAsync", "KeyNotFoundException")]
        [Theory(DisplayName = "Lança KeyNotFoundException e retorna 404")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task InvokeAsync_KeyNotFoundException_ReturnsNotFound(bool isDevelopment)
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            RequestDelegate next = (ctx) => throw new KeyNotFoundException("Cliente não encontrado");
            var middleware = new ExceptionMiddleware(next, _loggerMock.Object, isDevelopment);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Trait("InvokeAsync", "ArgumentException")]
        [Theory(DisplayName = "Lança ArgumentException e retorna 400")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task InvokeAsync_ArgumentException_ReturnsBadRequest(bool isDevelopment)
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            RequestDelegate next = (ctx) => throw new ArgumentException("Argumento inválido");
            var middleware = new ExceptionMiddleware(next, _loggerMock.Object, isDevelopment);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Trait("InvokeAsync", "InvalidOperationException")]
        [Theory(DisplayName = "Lança InvalidOperationException e retorna 400")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task InvokeAsync_InvalidOperationException_ReturnsBadRequest(bool isDevelopment)
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            RequestDelegate next = (ctx) => throw new InvalidOperationException("Operação inválida");
            var middleware = new ExceptionMiddleware(next, _loggerMock.Object, isDevelopment);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Trait("InvokeAsync", "Exception")]
        [Theory(DisplayName = "Lança Exception e retorna 500")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task InvokeAsync_Exception_ReturnsInternalServerError(bool isDevelopment)
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            RequestDelegate next = (ctx) => throw new Exception("Erro interno");
            var middleware = new ExceptionMiddleware(next, _loggerMock.Object, isDevelopment);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
