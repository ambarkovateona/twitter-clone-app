using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneApp.Application.Common.Exceptions;

namespace TwitterCloneApp.Api.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
{
    ValidationException => (StatusCodes.Status400BadRequest, "Validation error"),
    NotFoundException => (StatusCodes.Status404NotFound, "Not found"),
    ForbiddenAccessException => (StatusCodes.Status403Forbidden, "Forbidden"),
    _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
};

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message
        };

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions["errors"] = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}