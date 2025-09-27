using HelsiTestTask.Application.Features.TaskLists.Queries.Common.Dtos;
using HelsiTestTask.Application.Features.TaskLists.Queries.Common.Mappings;
using HelsiTestTask.Application.Mediator.Core;
using HelsiTestTask.Domain.Common.Result;
using HelsiTestTask.Domain.TaskLists;

namespace HelsiTestTask.Application.Features.TaskLists.Queries.GetTaskLists;

public class GetTaskListsQueryHandler: IRequestHandler<GetTaskListsQuery, Result<List<TaskListDto>>>
{
    private readonly ITaskListsRepository _taskListsRepository;

    public GetTaskListsQueryHandler(ITaskListsRepository taskListsRepository)
    {
        _taskListsRepository = taskListsRepository;
    }

    public async Task<Result<List<TaskListDto>>> Handle(GetTaskListsQuery request, CancellationToken cancellationToken)
    {
        var taskLists = await _taskListsRepository.GetByUserIdAsync(
            request.UserId,
            request.Descending,
            request.Skip,
            request.Take,
            cancellationToken);

        var dtos = taskLists
            .Select(t => t.ToDto(request.UserId))
            .ToList();
        
        return Result.Ok(dtos);
    }
}
