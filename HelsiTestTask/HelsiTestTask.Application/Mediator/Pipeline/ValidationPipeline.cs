using System.Runtime.CompilerServices;
using FluentValidation;
using HelsiTestTask.Application.Mediator.Core;
using HelsiTestTask.Domain.Common.Result;

namespace HelsiTestTask.Application.Mediator.Pipeline;
public class ValidationPipeline<TRequest, TResponse>: IPipelineBehavior<TRequest,TResponse> 
    where TRequest : IRequest<TResponse>
    where TResponse : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    
    public ValidationPipeline(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResult = _validators.Select(v => v.Validate(context));

            var failures = validationResult
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                var failedResult = Result.Fail(
                    failures.Select(f =>
                        new ValidationError(f.ErrorCode, f.ErrorMessage, f.PropertyName)));
                
                return Unsafe.As<TResponse>(failedResult);
            }
        }
        
        return await next();
    }
}
