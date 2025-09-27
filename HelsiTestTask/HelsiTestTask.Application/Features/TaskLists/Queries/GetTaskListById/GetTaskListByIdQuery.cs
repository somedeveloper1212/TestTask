using HelsiTestTask.Application.Features.TaskLists.Queries.Common.Dtos;
using HelsiTestTask.Application.Mediator.Core;
using HelsiTestTask.Domain.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace HelsiTestTask.Application.Features.TaskLists.Queries.GetTaskListById;

public class GetTaskListByIdQuery: IRequest<Result<TaskListDto>>
{
    [FromRoute] 
    public required string Id { get; init; }
    
    [FromHeader(Name = "X-User-Id")]
    public required string UserId { get; init; }
}
