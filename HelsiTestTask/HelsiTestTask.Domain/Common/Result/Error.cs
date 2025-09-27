namespace HelsiTestTask.Domain.Common.Result;

public abstract record Error
{
    private protected Error(string code, string message)
    {
        Message = message;
        Code = code;
    }
    
    private protected Error(string code, string message, string property)
    {
        Message = message;
        Property = property;
        Code = code;
    }
    public string Message { get; private set; }
    public string Code { get; private set; }
    public string? Property { get; private set; }
    
    public Exception? Exception { get; protected set; }
}

public record NotFound : Error{
    public NotFound(string code, string message) 
        : base(code, message) { }
}

public record Critical : Error
{
    public Critical(string message) 
        : base("Critical", message) { }

    public Critical WithException(Exception exception)
    {
        Exception = exception;
        return this;
    }
}

public record Unprocessable : Error
{
    public Unprocessable(string code, string message) 
        : base(code, message) { }
}

public record Conflict : Error{
    public Conflict(string code, string message) 
        : base(code, message) { }
}

public record ValidationError : Error{
    public ValidationError(string code, string message) 
        : base(code, message) { }

    public ValidationError(string code, string message, string property) 
        : base(code, message, property) { }
}

public record Unauthorized : Error{
    public Unauthorized(string code, string message) 
        : base(code, message) { }
}

public record Forbidden : Error{
    public Forbidden(string code, string message) 
        : base(code, message) { }
}
