using FraudSys.Domain.Resources;
using System.Net;
using System.Text.Json;

namespace FraudSys.Api.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, bool isDevelopment)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleException(context, ex, HttpStatusCode.NotFound);
            }
            catch (ArgumentException ex)
            {
                await HandleException(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex, HttpStatusCode httpStatusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;


            switch (httpStatusCode)
            {
                case HttpStatusCode.NotFound:
                    return;

                case HttpStatusCode.BadRequest:
                    logger.LogWarning(ex, ResourceMessages.FailExecuteRouteExceptionFilter, context.Request.Method, context.Request.Path);
                    break;

                case HttpStatusCode.InternalServerError:
                default:
                    logger.LogError(ex, ResourceMessages.FailExecuteRouteExceptionFilter, context.Request.Path);
                    break;
            }

            object response;
            if (isDevelopment)
            {
                response = new
                {
                    status = context.Response.StatusCode,
                    message = ex.Message,
                    detail = ex.StackTrace
                };
            }
            else
            {
                response = new
                {
                    status = context.Response.StatusCode,
                    message = ex.Message
                };
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

}
