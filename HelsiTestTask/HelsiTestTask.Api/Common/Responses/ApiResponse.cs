using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HelsiTestTask.Api.Common.Responses;

public class ApiResponse<T> 
{
    public T? Data { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
    public List<ApiError>? Error { get; set; }
}

public class ApiResponse
{
    public Dictionary<string, object>? Metadata { get; set; }
    public List<ApiError>? Error { get; set; }

    public static ApiResponse From(ModelStateDictionary modelState)
    {
        var errors = modelState
            .Where(entry => entry.Value is { Errors.Count: > 0 })
            .SelectMany(entry => entry.Value!.Errors.Select(error => new ApiError(
                message: error.ErrorMessage,
                errorCode: "ValidationError",
                propertyName: entry.Key
            )))
            .ToList();

        return new ApiResponse
        {
            Metadata = null,
            Error = errors
        };
    }
}