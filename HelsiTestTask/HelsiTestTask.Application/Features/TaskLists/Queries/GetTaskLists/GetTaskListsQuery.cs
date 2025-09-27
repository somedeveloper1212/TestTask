using HelsiTestTask.Application.Features.TaskLists.Queries.Common.Dtos;
using HelsiTestTask.Application.Mediator.Core;
using HelsiTestTask.Domain.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace HelsiTestTask.Application.Features.TaskLists.Queries.GetTaskLists;

public class GetTaskListsQuery: IRequest<Result<List<TaskListDto>>>
{
    [FromHeader(Name = "X-User-Id")]
    public required string UserId { get; init; }
    
    [FromQuery] 
    public int Skip { get; init; } = 0;
    
    [FromQuery] 
    public int Take { get; init; } = 10;
    
    [FromQuery]
    public bool Descending { get; init; } = true;
}
