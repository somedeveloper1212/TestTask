namespace HelsiTestTask.Domain.Common.Result;

public interface IResultMetadata
{
    public Dictionary<string, object> Value { get; }
}