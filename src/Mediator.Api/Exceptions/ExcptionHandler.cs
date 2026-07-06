using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;


namespace Mediator.Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception processing {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);

        var problem = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        switch (exception)
        {
            case ArgumentNullException:
                problem.Title = "Invalid request";
                problem.Detail = exception.Message;
                problem.Status = StatusCodes.Status400BadRequest;
                break;

            case KeyNotFoundException:
                problem.Title = "Resource not found";
                problem.Detail = exception.Message;
                problem.Status = StatusCodes.Status404NotFound;
                break;
            case FluentValidation.ValidationException ex:

                problem.Title = "Validation failed";
                problem.Status = StatusCodes.Status400BadRequest;

                problem.Extensions["errors"] =
                    ex.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage));

                break;
            default:
                problem.Title = "Server Error";
                problem.Detail = "An unexpected error occurred.";
                problem.Status = StatusCodes.Status500InternalServerError;
                break;
        }

        httpContext.Response.StatusCode = problem.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}