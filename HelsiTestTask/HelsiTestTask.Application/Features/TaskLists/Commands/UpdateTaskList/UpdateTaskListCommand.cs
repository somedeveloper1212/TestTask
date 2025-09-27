using HelsiTestTask.Application.Mediator.Core;
using HelsiTestTask.Domain.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace HelsiTestTask.Application.Features.TaskLists.Commands.UpdateTaskList;

public class UpdateTaskListCommand: IRequest<Result>
{
    [FromRoute] 
    public required string Id { get; init; }

    [FromHeader(Name = "X-User-Id")]
    public required string UserId { get; init; }
    
    [FromBody] 
    public required UpdateTaskListBody Body { get; init; }
}

public class UpdateTaskListBody
{
    public required string Name { get; init; }
}
