using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("Error Message: {exceptionMessage}, Time of occurrence: {time}", 
            exception.Message, DateTime.Now);

        (string Detail, string Title, int StatusCode) details = exception switch
        {
            InternalServerException => (exception.Message, exception.GetType().Name, StatusCodes.Status500InternalServerError),
            ValidationException => (exception.Message, exception.GetType().Name, StatusCodes.Status400BadRequest),
            BadRequestException => (exception.Message, exception.GetType().Name, StatusCodes.Status400BadRequest),
            BadHttpRequestException => (exception.Message, exception.GetType().Name, StatusCodes.Status400BadRequest),  // Guid if not valid
            NotFoundException => (exception.Message, exception.GetType().Name, StatusCodes.Status404NotFound),
            _ => (exception.Message, exception.GetType().Name, StatusCodes.Status500InternalServerError)
        };

        httpContext.Response.StatusCode = details.StatusCode;  // Set the status code, or will get 500 by default

        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Detail,
            Status = details.StatusCode,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);
        if(exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails);
        return true;
    }
}

