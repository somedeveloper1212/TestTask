using HelsiTestTask.Application.Mediator.Core;
using HelsiTestTask.Domain.Common.Result;
using HelsiTestTask.Domain.TaskLists;

namespace HelsiTestTask.Application.Features.TaskLists.Commands.UpdateTaskListShare;

public class UpdateTaskListShareCommandHandler: IRequestHandler<UpdateTaskListShareCommand, Result>
{
    private readonly ITaskListsRepository _taskListsRepository;

    public UpdateTaskListShareCommandHandler(ITaskListsRepository taskListsRepository)
    {
        _taskListsRepository = taskListsRepository;
    }
    
    public async Task<Result> Handle(UpdateTaskListShareCommand request, CancellationToken cancellationToken)
    {
        var taskList = await _taskListsRepository.GetByIdAsync(request.Id, cancellationToken);
        
         if (taskList is null)
             return Result.Fail(TaskListErrors.TaskListNotFound(request.Id));
        
         if (taskList.CreatedById != request.RequestedById)
             return Result.Fail(TaskListErrors.OnlyOwnerCanDoThis);
         
         if (taskList.CreatedById == request.UserId)
             return Result.Fail(TaskListErrors.CannotShareWithOwner(request.UserId, taskList.Id));
         
        await UpdateTaskListShareAsync(taskList, request, cancellationToken);
        
        return Result.Ok();
    }
    
    private async Task UpdateTaskListShareAsync(
        TaskList taskList,
        UpdateTaskListShareCommand request,
        CancellationToken cancellationToken)
    {
        var isChanged = request.Grant
            ? taskList.SharedWithUsersIds.Add(request.UserId)
            : taskList.SharedWithUsersIds.Remove(request.UserId);

        if (isChanged)
        {
            taskList.UpdatedAtUtc = DateTime.UtcNow;
            await _taskListsRepository.UpdateAsync(taskList, cancellationToken);
        }
    }
}
