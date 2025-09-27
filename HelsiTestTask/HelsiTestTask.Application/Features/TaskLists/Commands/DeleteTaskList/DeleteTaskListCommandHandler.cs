using HelsiTestTask.Application.Mediator.Core;
using HelsiTestTask.Domain.Common.Result;
using HelsiTestTask.Domain.TaskLists;

namespace HelsiTestTask.Application.Features.TaskLists.Commands.DeleteTaskList;

public class DeleteTaskListCommandHandler: IRequestHandler<DeleteTaskListCommand, Result>
{
    private readonly ITaskListsRepository _taskListsRepository;

    public DeleteTaskListCommandHandler(ITaskListsRepository taskListsRepository)
    {
        _taskListsRepository = taskListsRepository;
    }

    public async Task<Result> Handle(DeleteTaskListCommand request, CancellationToken cancellationToken)
    {
        var taskList = await _taskListsRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (taskList is null)
            return Result.Fail(TaskListErrors.TaskListNotFound(request.Id));
        
        if (taskList.CreatedById != request.ClientId)
            return Result.Fail(TaskListErrors.OnlyOwnerCanDoThis);
        
        await _taskListsRepository.DeleteAsync(taskList.Id, cancellationToken);
        
        return Result.Ok();
    }
}
