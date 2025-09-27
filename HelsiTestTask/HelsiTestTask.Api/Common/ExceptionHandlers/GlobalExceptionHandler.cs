using HelsiTestTask.Api.Common.Responses;
using Microsoft.AspNetCore.Diagnostics;

namespace HelsiTestTask.Api.Common.ExceptionHandlers;

public class GlobalExceptionHandler: IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private const string ErrorCode = "UnhandledException";

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogCritical(exception, "Unhandled exception in GlobalExceptionHandler: {Exception}", exception);

        var rootException = exception.GetBaseException();

        var response = new ApiResponse
        {
            Error = [new ApiError(rootException.Message, ErrorCode)]
        };
        
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken: cancellationToken);
        return true;
    }
}
