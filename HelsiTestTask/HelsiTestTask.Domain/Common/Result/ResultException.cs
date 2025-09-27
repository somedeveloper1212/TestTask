namespace HelsiTestTask.Domain.Common.Result;

public class ResultException : Exception
{
    private readonly List<Error> _errors;

    public ResultException(Error error)
    {
        _errors = [error];
    }

    public ResultException(List<Error> errors)
    {
        _errors = errors;
    }

    public Result ToResult()
    {
        return Result.Fail(_errors);
    }
}
