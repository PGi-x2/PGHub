using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PGHub.Application.Exceptions;
using System.Net;

namespace PGHub.Application.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";

            switch (exception)
            {
                case BadRequestException badRequestException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await httpContext.Response.WriteAsJsonAsync(new
                    {
                        // infer the type of the object
                        httpContext.Response.StatusCode,
                        badRequestException.Message
                    });
                    _logger.LogError(badRequestException.Message);
                    break;
                case NotFoundException notFoundException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    await httpContext.Response.WriteAsJsonAsync(new
                    {
                        // infer the type of the object
                        httpContext.Response.StatusCode,
                        notFoundException.Message
                    });
                    _logger.LogError(notFoundException.Message);
                    break;
                case ValidationException validationException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await httpContext.Response.WriteAsJsonAsync(new
                    {
                        // infer the type of the object
                        httpContext.Response.StatusCode,
                        validationException.Errors
                    });
                    _logger.LogError(validationException.Message);
                    break;
                default:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await httpContext.Response.WriteAsJsonAsync(new
                    {
                        // infer the type of the object
                        httpContext.Response.StatusCode,
                        Message = "An error occurred while processing the request."
                    });
                    _logger.LogError(exception.Message);
                    break;
            }
        }
    }
}
