using BuildingBlocks.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using BuildingBlocks.Domain.Exceptions;

namespace BuildingBlocks.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception.Message);

            var (message, statusCode) = MapException(exception);
            var response = new
            {
                message,
                statusCode
            };
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static (string message, int statusCode) MapException(Exception exception)
        {
            return exception switch
            {
                AppException appEx =>
                    (appEx.Message, (int)appEx.StatusCode),
                
                DomainException domainEx =>
                    (domainEx.Message, StatusCodes.Status400BadRequest),

                FluentValidation.ValidationException valEx =>
                    (valEx.Errors.FirstOrDefault()?.ErrorMessage ?? "Dữ liệu không hợp lệ", 400),

                _ =>
                    ("Đã có lỗi từ server", 500)
            };
        }
    }
}
