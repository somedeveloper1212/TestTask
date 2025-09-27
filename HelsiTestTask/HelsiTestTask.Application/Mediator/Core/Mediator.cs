using HelsiTestTask.Application.Mediator.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace HelsiTestTask.Application.Mediator.Core;
public class Mediator: IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        var handler = _serviceProvider.GetRequiredService(handlerType);
        var handleMethod = handlerType.GetMethod("Handle") 
                           ?? throw new InvalidOperationException($"Handle method not found for handler {handlerType.Name}");

        RequestHandlerDelegate<TResponse> handlerDelegate = () =>
            (Task<TResponse>)handleMethod.Invoke(handler, [request, cancellationToken])!;

        var pipelineType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviors = _serviceProvider.GetServices(pipelineType);

        foreach (var behavior in behaviors.Reverse())
        {
            var next = handlerDelegate;
            handlerDelegate = () =>
            {
                var behaviorMethod = behavior?.GetType().GetMethod("Handle") 
                                     ?? throw new InvalidOperationException($"Handle method not found on pipeline {behavior?.GetType().Name}");

                return (Task<TResponse>)behaviorMethod.Invoke(behavior, [request, next, cancellationToken])!;
            };
        }

        return await handlerDelegate();
    }
}

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();