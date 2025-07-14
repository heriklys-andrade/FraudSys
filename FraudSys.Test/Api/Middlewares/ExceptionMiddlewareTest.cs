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
        [Fact(DisplayName = "Executa com sucesso uma requisição HTTP")]
        public async Task InvokeAsync_Success()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            RequestDelegate next = async (ctx) =>
            {
                ctx.Response.StatusCode = StatusCodes.Status200OK;
                await ctx.Response.WriteAsync("Sucesso");
            };

            var middleware = new ExceptionMiddleware(next, _loggerMock.Object, true);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Trait("InvokeAsync", "KeyNotFoundException")]
        [Fact(DisplayName = "Lança KeyNotFoundException e retorna 404")]
        public async Task InvokeAsync_KeyNotFoundException_ReturnsNotFound()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            RequestDelegate next = (ctx) => throw new KeyNotFoundException("Cliente não encontrado");
            var middleware = new ExceptionMiddleware(next, _loggerMock.Object, true);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Trait("InvokeAsync", "ArgumentException")]
        [Fact(DisplayName = "Lança ArgumentException e retorna 400")]
        public async Task InvokeAsync_ArgumentException_ReturnsBadRequest()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            RequestDelegate next = (ctx) => throw new ArgumentException("Argumento inválido");
            var middleware = new ExceptionMiddleware(next, _loggerMock.Object, true);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Trait("InvokeAsync", "InvalidOperationException")]
        [Fact(DisplayName = "Lança InvalidOperationException e retorna 400")]
        public async Task InvokeAsync_InvalidOperationException_ReturnsBadRequest()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            RequestDelegate next = (ctx) => throw new InvalidOperationException("Operação inválida");
            var middleware = new ExceptionMiddleware(next, _loggerMock.Object, true);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Trait("InvokeAsync", "Exception")]
        [Fact(DisplayName = "Lança Exception e retorna 500")]
        public async Task InvokeAsync_Exception_ReturnsInternalServerError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            RequestDelegate next = (ctx) => throw new Exception("Erro interno");
            var middleware = new ExceptionMiddleware(next, _loggerMock.Object, true);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
