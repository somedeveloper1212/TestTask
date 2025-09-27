using HelsiTestTask.Application.Features.TaskLists.Queries.Common.Dtos;
using HelsiTestTask.Application.Features.TaskLists.Queries.Common.Mappings;
using HelsiTestTask.Application.Mediator.Core;
using HelsiTestTask.Domain.Common.Result;
using HelsiTestTask.Domain.TaskLists;

namespace HelsiTestTask.Application.Features.TaskLists.Queries.GetTaskListShares;

public class GetTaskListSharesQueryHandler: IRequestHandler<GetTaskListSharesQuery, Result<List<TaskListShareUserDto>>>
{
    private readonly ITaskListsRepository _taskListsRepository;

    public GetTaskListSharesQueryHandler(ITaskListsRepository taskListsRepository)
    {
        _taskListsRepository = taskListsRepository;
    }

    public async Task<Result<List<TaskListShareUserDto>>> Handle(
        GetTaskListSharesQuery request,
        CancellationToken cancellationToken)
    {
        var taskList = await _taskListsRepository.GetByIdAsync(request.Id, cancellationToken);

        if (taskList is null)
            return TaskListErrors.TaskListNotFound(request.Id);

        var isInSharedUsers = taskList.SharedWithUsersIds.Any(id => id== request.UserId);
        
        if (taskList.CreatedById != request.UserId && !isInSharedUsers)
            return TaskListErrors.TaskListAccessDenied;

        var dtos = taskList.ToShareDtos();
        
        return Result.Ok(dtos);
    }
}
