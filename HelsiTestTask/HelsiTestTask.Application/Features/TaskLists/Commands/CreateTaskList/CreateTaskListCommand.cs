using HelsiTestTask.Application.Mediator.Core;
using HelsiTestTask.Domain.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace HelsiTestTask.Application.Features.TaskLists.Commands.CreateTaskList;

public class CreateTaskListCommand: IRequest<Result<CreateTaskListCommandResponse>>
{
    [FromHeader(Name = "X-User-Id")]
    public required string UserId { get; init; }
    
    [FromBody] 
    public required CreateTaskListBody Body { get; init; }
}

public class CreateTaskListBody
{
    public required string Name { get; init; }
}

public record CreateTaskListCommandResponse(string Id);
