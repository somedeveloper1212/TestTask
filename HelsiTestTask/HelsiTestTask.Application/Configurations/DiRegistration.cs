using FluentValidation;
using HelsiTestTask.Application.Features.TaskLists.Commands.CreateTaskList;
using HelsiTestTask.Application.Features.TaskLists.Commands.DeleteTaskList;
using HelsiTestTask.Application.Features.TaskLists.Commands.UpdateTaskList;
using HelsiTestTask.Application.Features.TaskLists.Commands.UpdateTaskListShare;
using HelsiTestTask.Application.Features.TaskLists.Queries.Common.Dtos;
using HelsiTestTask.Application.Features.TaskLists.Queries.GetTaskListById;
using HelsiTestTask.Application.Features.TaskLists.Queries.GetTaskLists;
using HelsiTestTask.Application.Features.TaskLists.Queries.GetTaskListShares;
using HelsiTestTask.Application.Mediator.Core;
using HelsiTestTask.Application.Mediator.Pipeline;
using HelsiTestTask.Domain.Common.Result;
using Microsoft.Extensions.DependencyInjection;

namespace HelsiTestTask.Application.Configurations;

public static class DiRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator.Core.Mediator>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));
        
        services.AddValidatorsFromAssembly(typeof(DiRegistration).Assembly);

        services.AddScoped<IRequestHandler<CreateTaskListCommand, Result<CreateTaskListCommandResponse>>, CreateTaskListCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteTaskListCommand, Result>, DeleteTaskListCommandHandler>();
        services.AddScoped<IRequestHandler<CreateTaskListCommand, Result<CreateTaskListCommandResponse>>, CreateTaskListCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateTaskListCommand, Result>, UpdateTaskListCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateTaskListShareCommand, Result>, UpdateTaskListShareCommandHandler>();
        
        services.AddScoped<IRequestHandler<GetTaskListByIdQuery, Result<TaskListDto>>, GetTaskListByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetTaskListsQuery, Result<List<TaskListDto>>>, GetTaskListsQueryHandler>();
        services.AddScoped<IRequestHandler<GetTaskListSharesQuery, Result<List<TaskListShareUserDto>>>, GetTaskListSharesQueryHandler>();

        return services;
    }
}
