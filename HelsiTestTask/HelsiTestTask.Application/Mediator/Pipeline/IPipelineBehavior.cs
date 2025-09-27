using HelsiTestTask.Application.Mediator.Core;

namespace HelsiTestTask.Application.Mediator.Pipeline;

public interface IPipelineBehavior<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}