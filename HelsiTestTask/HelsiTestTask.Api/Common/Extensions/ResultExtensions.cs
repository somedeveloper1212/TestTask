using HelsiTestTask.Api.Common.Responses;
using HelsiTestTask.Domain.Common.Result;

namespace HelsiTestTask.Api.Common.Extensions;

public static class ResultExtensions
{
    public static ApiResponse<T> MapToApiResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return result.Metadata != null
                ? new ApiResponse<T>
                {
                    Data = result.Value,
                    Metadata = result.Metadata.Value
                }
                : new ApiResponse<T>
                {
                    Data = result.Value,
                };
        }
    
        return new ApiResponse<T>
        {
            Error = result.Errors
                .Select(e => new ApiError(e.Message, e.Code.GetType().Name,e.Property))
                .ToList()
        };
    }
}
