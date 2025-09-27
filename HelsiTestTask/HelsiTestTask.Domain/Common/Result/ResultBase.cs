namespace HelsiTestTask.Domain.Common.Result;

public interface IResultBase
{
    /// <summary>
    /// Is true if Reasons contains at least one error
    /// </summary>
    bool IsFailed { get; }

    /// <summary>
    /// Is true if Reasons contains no errors
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// All action errors
    /// </summary>
    List<Error> Errors { get; }
}

/// <summary>
/// Default implementation of <see cref="IResultBase"/>
/// </summary>
public abstract class ResultBase : IResultBase
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public bool IsFailed => Errors.Any();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public bool IsSuccess => !IsFailed;

    public List<Error> Errors { get; protected set; }
    
    public IResultMetadata? Metadata { get; protected set; }


    /// <summary>
    /// Default constructor
    /// </summary>
    protected ResultBase()
    {
        Errors = [];
    }

    /// <summary>
    /// Check if the result object contains an exception from a specific type
    /// </summary>
    public bool HasException<TException>() where TException : Exception
    {
        return HasException<TException>(_ => true);
    }

    /// <summary>
    /// Check if the result object contains an exception from a specific type and with a specific condition
    /// </summary>
    public bool HasException<TException>(Func<TException, bool> predicate) where TException : Exception
    {
        return Errors.Where(e => e.Exception != null)
            .Select(e=>e.Exception)
            .Cast<TException>()
            .Any(predicate);
    }
}

/// <summary>
/// Default implementation of <see cref="IResultBase"/> generics
/// </summary>
public abstract class ResultBase<TResult> : ResultBase
    where TResult : ResultBase<TResult>;