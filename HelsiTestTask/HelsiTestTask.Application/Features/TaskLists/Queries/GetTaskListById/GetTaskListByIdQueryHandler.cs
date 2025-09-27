using HelsiTestTask.Application.Features.TaskLists.Queries.Common.Dtos;
using HelsiTestTask.Application.Features.TaskLists.Queries.Common.Mappings;
using HelsiTestTask.Application.Mediator.Core;
using HelsiTestTask.Domain.Common.Result;
using HelsiTestTask.Domain.TaskLists;

namespace HelsiTestTask.Application.Features.TaskLists.Queries.GetTaskListById;

public class GetTaskListByIdQueryHandler: IRequestHandler<GetTaskListByIdQuery, Result<TaskListDto>>
{
    private readonly ITaskListsRepository _taskListsRepository;

    public GetTaskListByIdQueryHandler(ITaskListsRepository taskListsRepository)
    {
        _taskListsRepository = taskListsRepository;
    }
    
    public async Task<Result<TaskListDto>> Handle(GetTaskListByIdQuery request, CancellationToken cancellationToken)
    {
        var taskList = await _taskListsRepository.GetByIdAsync(request.Id, cancellationToken);

        if (taskList is null)
            return TaskListErrors.TaskListNotFound(request.Id);

        var isOwner = taskList.CreatedById == request.UserId;
        var isInSharedUsers = taskList.SharedWithUsersIds.Any(id => id== request.UserId);
        
        if (!isOwner && !isInSharedUsers)
            return TaskListErrors.TaskListAccessDenied;

        var dtos = taskList.ToDto(request.UserId);
        
        return Result.Ok(dtos);
    }
}
