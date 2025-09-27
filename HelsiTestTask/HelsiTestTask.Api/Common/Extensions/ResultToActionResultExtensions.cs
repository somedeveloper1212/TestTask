using HelsiTestTask.Api.Common.Responses;
using HelsiTestTask.Domain.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace HelsiTestTask.Api.Common.Extensions;

public static class ResultToActionResultExtensions
{
    public static IActionResult ToActionResult<TValue>(this Result<TValue> result,
        Func<Result<TValue>, IActionResult> onSuccess)
    {
        return result switch
        {
            { IsSuccess: true } => onSuccess(result),
            { IsFailed: true } => result.AsFailedActionResult(),
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };
    }
    
    public static IActionResult ToActionResult(this Result result,
        Func<Result, IActionResult> onSuccess)
    {
        return result switch
        {
            { IsSuccess: true } => onSuccess(result),
            { IsFailed: true } => result.AsFailedActionResult(),
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };
    }
    
    private static IActionResult AsFailedActionResult<T>(this Result<T> result) => AsFailedActionResult(result.ToResult());
    
    private static IActionResult AsFailedActionResult(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("You cannot create a failed result from a Success");
        
        
        var firstError = result.Errors.FirstOrDefault();
        var response = new ApiResponse
        {
            Error = result.Errors
                .Select(e => new ApiError(e.Message, e.Code,e.Property))
                .ToList()
        };
        
        var actionResult = firstError switch
        {
            Conflict => new ObjectResult(response){StatusCode = StatusCodes.Status409Conflict},
            Critical => new ObjectResult(response){StatusCode = StatusCodes.Status500InternalServerError},
            NotFound => new ObjectResult(response){StatusCode = StatusCodes.Status404NotFound},
            Unauthorized => new ObjectResult(response){StatusCode = StatusCodes.Status401Unauthorized},
            Unprocessable => new ObjectResult(response){StatusCode = StatusCodes.Status422UnprocessableEntity},
            ValidationError => new ObjectResult(response){StatusCode = StatusCodes.Status400BadRequest},
            Forbidden => new ObjectResult(response){StatusCode = StatusCodes.Status403Forbidden},
            _ => throw new ArgumentOutOfRangeException(nameof(result))
        };
        
        return actionResult;
    }
}
