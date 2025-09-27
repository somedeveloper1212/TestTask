namespace HelsiTestTask.Domain.Common.Result;

/// <summary>
/// Implementation of a Result
/// </summary>
public class Result : ResultBase<Result>
{
    public Result()
    { }

    public Result<TNewValue> ToResult<TNewValue>(TNewValue newValue)
    {
        return new Result<TNewValue>()
            .WithValue((IsFailed ? default : newValue)!)
            .WithErrors(Errors);
    }
    
    public Result WithErrors(params Error [] errors)
    {
        Errors = errors.ToList();
        return this;
    }
    
    public Result WithErrors(List<Error> errors)
    {
        Errors = errors;
        return this;
    }

    public Result WithMetadata(IResultMetadata metadata)
    {
        Metadata = metadata;
        return this;
    }
    
    public static Result Ok()
    {
        return new Result();
    }
     
    public static Result<TValue> Ok<TValue>(TValue value)
    {
        var result = new Result<TValue>();
        result.WithValue(value);
        return result;
    }

    public static Result Fail(Error error)
    {
        var result = new Result();
        result.WithErrors(error);
        return result;
    }

    public static Result Fail(IEnumerable<Error> errors)
    {
        if (errors == null)
            throw new ArgumentNullException(nameof(errors), "The list of errors cannot be null");

        var result = new Result();
        result.WithErrors(errors.ToArray());
        return result;
    }

    public static Result<TValue> Fail<TValue>(Error error)
    {
        var result = new Result<TValue>();
        result.WithErrors(error);
        return result;
    }

    public static Result<TValue> Fail<TValue>(params Error[] errors)
    {
        if (errors == null)
            throw new ArgumentNullException(nameof(errors), "The list of error messages cannot be null");

        var result = new Result<TValue>();
        result.WithErrors(errors);
        return result;
    }

    public static Result<TValue> Fail<TValue>(IEnumerable<Error> errors)
    {
        if (errors == null)
            throw new ArgumentNullException(nameof(errors), "The list of errors cannot be null");

        var result = new Result<TValue>();
        result.WithErrors(errors.ToList());
        return result;
    }

    public Result<TNewValue> Bind<TNewValue>(Func<Result<TNewValue>> bind)
    {
        var result = new Result<TNewValue>();
        result.WithErrors(Errors);
        
        if (IsSuccess)
        {
            var converted = bind();
            result.WithValue(converted.ValueOrDefault);
            result.WithErrors(Errors);
        }

        return result;
    }

    public async Task<Result<TNewValue>> Bind<TNewValue>(Func<Task<Result<TNewValue>>> bind)
    {
        var result = new Result<TNewValue>();
        result.WithErrors(Errors);
        
        if (IsSuccess)
        {
            var converted = await bind();
            result.WithValue(converted.ValueOrDefault);
            result.WithErrors(Errors);
        }

        return result;
    }
    
    public async ValueTask<Result<TNewValue>> Bind<TNewValue>(Func<ValueTask<Result<TNewValue>>> bind)
    {
        var result = new Result<TNewValue>();
        result.WithErrors(Errors);
        
        if (IsSuccess)
        {
            var converted = await bind();
            result.WithValue(converted.ValueOrDefault);
            result.WithErrors(Errors);
        }

        return result;
    }
    
    public Result Bind(Func<Result> action)
    {
        var result = new Result();
        result.WithErrors(Errors);
        
        if (IsSuccess)
        {
            var converted = action();
            result.WithErrors(converted.Errors);
        }

        return result;
    }
    
    public async Task<Result> Bind(Func<Task<Result>> action)
    {
        var result = new Result();
        result.WithErrors(Errors);
        
        if (IsSuccess)
        {
            var converted = await action();
            result.WithErrors(converted.Errors);
        }

        return result;
    }
    
    public async ValueTask<Result> Bind(Func<ValueTask<Result>> action)
    {
        var result = new Result();
        result.WithErrors(Errors);
        
        if (IsSuccess)
        {
            var converted = await action();
            result.WithErrors(converted.Errors);
        }

        return result;
    }
    
    public static implicit operator Result(Error error)
    {
        return Fail(error);
    }

    public static implicit operator Result(List<Error> errors)
    {
        return Fail(errors);
    }
}

public interface IResult<out TValue> : IResultBase
{
    TValue Value { get; }

    TValue ValueOrDefault { get; }
}

public class Result<TValue> : ResultBase<Result<TValue>>, IResult<TValue>
{

    #pragma warning disable CS8618, CS9264
    internal Result()
    #pragma warning restore CS8618, CS9264
    { }

    private TValue _value;

    public TValue ValueOrDefault => _value;

    public TValue Value
    {
        get
        {
            ThrowIfFailed();

            return _value;
        }
        private set
        {
            ThrowIfFailed();

            _value = value;
        }
    }

    public Result<TValue> WithValue(TValue value)
    {
        Value = value;
        return this;
    }
    
    public Result<TValue> WithErrors(params Error [] errors)
    {
        Errors = errors.ToList();
        return this;
    }
    
    public Result<TValue> WithErrors(List<Error> errors)
    {
        Errors = errors;
        return this;
    }

    public Result<TValue> WithMetadata(IResultMetadata metadata)
    {
        Metadata = metadata;
        return this;
    }
    
    public Result ToResult()
    {
        return new Result()
            .WithErrors(Errors.ToArray());
    }

    public Result<TNewValue> ToResult<TNewValue>(Func<TValue, TNewValue>? valueConverter = null)
    {
        return Map(valueConverter);
    }

    public Result<TNewValue> Map<TNewValue>(Func<TValue, TNewValue>? mapLogic)
    {
        if (IsSuccess && mapLogic == null)
            throw new ArgumentException("If result is success then valueConverter should not be null");

        return new Result<TNewValue>()
               .WithValue((IsFailed ? default : mapLogic!(Value))!);
    }

    public Result<TNewValue> Bind<TNewValue>(Func<TValue, Result<TNewValue>> bind)
    {
        var result = new Result<TNewValue>();
        result.WithErrors(Errors);
        
        if (IsSuccess)
        {
            var converted = bind(Value);
            result.WithValue(converted.ValueOrDefault);
            result.WithErrors(Errors);
        }

        return result;
    }
    
    public async Task<Result<TNewValue>> Bind<TNewValue>(Func<TValue, Task<Result<TNewValue>>> bind)
    {
        var result = new Result<TNewValue>();
        result.WithErrors(Errors);
        
        if (IsSuccess)
        {
            var converted = await bind(Value);
            result.WithValue(converted.ValueOrDefault);
            result.WithErrors(Errors);
        }

        return result;
    }
    
    public async ValueTask<Result<TNewValue>> Bind<TNewValue>(Func<TValue, ValueTask<Result<TNewValue>>> bind)
    {
        var result = new Result<TNewValue>();
        result.WithErrors(Errors);
        
        if (IsSuccess)
        {
            var converted = await bind(Value);
            result.WithValue(converted.ValueOrDefault);
            result.WithErrors(Errors);
        }

        return result;
    }

    public Result Bind(Func<TValue, Result> action)
    {
        var result = new Result();
        result.WithErrors(Errors);

        if (IsSuccess)
        {
            var converted = action(Value);
            result.WithErrors(converted.Errors);
        }

        return result;
    }
    
    public async Task<Result> Bind(Func<TValue, Task<Result>> action)
    {
        var result = new Result();
        result.WithErrors(Errors);
        
        if (IsSuccess)
        {
            var converted = await action(Value);
            result.WithErrors(converted.Errors);
        }

        return result;
    }
    
    public async ValueTask<Result> Bind(Func<TValue, ValueTask<Result>> action)
    {
        var result = new Result();
        result.WithErrors(Errors);
        
        if (IsSuccess)
        {
            var converted = await action(Value);
            result.WithErrors(converted.Errors);
        }

        return result;
    }

    public static implicit operator Result<TValue>(Result result)
    {
        return result.ToResult<TValue>(default);
    }

    public static implicit operator Result<object>(Result<TValue> result)
    {
        return result.ToResult<object>(value => value);
    }

    public static implicit operator Result<TValue>(TValue value)
    {
        if (value is Result<TValue> r)
            return r;

        return Result.Ok(value);
    }
    
    public static implicit operator Result<TValue>(Error error)
    {
        return Result.Fail(error);
    }

    public static implicit operator Result<TValue>(List<Error> errors)
    {
        return Result.Fail(errors);
    }

    private void ThrowIfFailed()
    {
        if (IsFailed)
            throw new ResultException(new Critical("Result is in status failed. Value is not set."));
    }
}
