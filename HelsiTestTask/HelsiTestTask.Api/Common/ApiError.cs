namespace HelsiTestTask.Api.Common;

public struct ApiError
{
    public ApiError(string message, string errorCode, string? propertyName = null)
    {
        Message = message;
        ErrorCode = errorCode;
        PropertyName = propertyName;
    }

    public string Message { get;  }
    public string ErrorCode { get; }
    public string? PropertyName { get; }
}