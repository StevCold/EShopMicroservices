using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

using ValidationException = FluentValidation.ValidationException;

namespace BuildingBlocks.Exceptions.Handler;

public sealed class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception,
            "Error message: {exceptionMessage}, Time of occurance {time}",
            exception.Message, DateTime.UtcNow);

        // Always respond as ProblemDetails JSON
        context.Response.ContentType = "application/problem+json";

        (string Detail, string Title, int StatusCode, object? Errors) details = exception switch
        {
            // ✅ JSON/body issues (e.g., missing required JSON members, malformed JSON)
            JsonException => (
                "Invalid request body. Check JSON format and required fields.",
                "BadRequest",
                StatusCodes.Status400BadRequest,
                null
            ),

            // ✅ Invalid request body / content-length / etc.
            BadHttpRequestException badHttp => (
                badHttp.Message,
                "BadRequest",
                StatusCodes.Status400BadRequest,
                null
            ),

            // ✅ FluentValidation exception (very common in APIs)
            ValidationException fv when fv.Errors?.Any() == true => (
                "One or more validation errors occurred.",
                fv.GetType().Name,
                StatusCodes.Status400BadRequest,
                fv.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray())
            ),

            // ✅ DataAnnotations validation (if you manually throw it somewhere)
            ValidationException dataAnno => (
                dataAnno.Message,
                dataAnno.GetType().Name,
                StatusCodes.Status400BadRequest,
                new { ValidationErrors = dataAnno.Message }
            ),

            // ✅ Your custom exceptions
            BadRequestException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status400BadRequest,
                null
            ),
            NotFoundException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status404NotFound,
                null
            ),
            InternalServerException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status500InternalServerError,
                null
            ),

            // ✅ Fallback
            _ => (
                "An unexpected error occurred.",
                exception.GetType().Name,
                StatusCodes.Status500InternalServerError,
                null
            )
        };

        context.Response.StatusCode = details.StatusCode;

        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Detail,
            Status = details.StatusCode,
            Instance = context.Request.Path
        };

        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        if (details.Errors is not null)
        {
            problemDetails.Extensions["errors"] = details.Errors;
        }

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}