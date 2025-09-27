using HelsiTestTask.Application.Mediator.Core;
using HelsiTestTask.Domain.Common.Result;
using HelsiTestTask.Domain.TaskLists;

namespace HelsiTestTask.Application.Features.TaskLists.Commands.CreateTaskList;

public class CreateTaskListCommandHandler: IRequestHandler<CreateTaskListCommand, Result<CreateTaskListCommandResponse>>
{
    private readonly ITaskListsRepository _taskListsRepository;

    public CreateTaskListCommandHandler(ITaskListsRepository taskListsRepository)
    {
        _taskListsRepository = taskListsRepository;
    }

    public async Task<Result<CreateTaskListCommandResponse>> Handle(CreateTaskListCommand request, CancellationToken cancellationToken)
    {
        var body = request.Body;
        var taskListId = Guid.NewGuid().ToString();

        var taskListResult = TaskList.TryCreate(
            id: taskListId,
            name: body.Name,
            createdByUserId: request.UserId
        );
        
        if (taskListResult.IsFailed)
            return taskListResult.ToResult();
        
        await _taskListsRepository.CreateAsync(taskListResult.Value, cancellationToken);
        
        return Result.Ok(new CreateTaskListCommandResponse(taskListId));
    }
}
