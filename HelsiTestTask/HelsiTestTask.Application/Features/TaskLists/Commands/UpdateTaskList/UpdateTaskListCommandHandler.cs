using HelsiTestTask.Application.Mediator.Core;
using HelsiTestTask.Domain.Common.Result;
using HelsiTestTask.Domain.TaskLists;

namespace HelsiTestTask.Application.Features.TaskLists.Commands.UpdateTaskList;

public class UpdateTaskListCommandHandler: IRequestHandler<UpdateTaskListCommand, Result>
{
    private readonly ITaskListsRepository _taskListsRepository;

    public UpdateTaskListCommandHandler(ITaskListsRepository taskListsRepository)
    {
        _taskListsRepository = taskListsRepository;
    }
    
    public async Task<Result> Handle(UpdateTaskListCommand request, CancellationToken cancellationToken)
    {
        var body = request.Body;
        var taskList = await _taskListsRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (taskList is null)
            return Result.Fail(TaskListErrors.TaskListNotFound(request.Id));
        
        if (taskList.CreatedById != request.UserId)
            return Result.Fail(TaskListErrors.TaskListAccessDenied);
        
        taskList.Name = body.Name;
        taskList.UpdatedAtUtc = DateTime.UtcNow;
        
        await _taskListsRepository.UpdateAsync(taskList, cancellationToken);
        
        return Result.Ok();
    }
}
