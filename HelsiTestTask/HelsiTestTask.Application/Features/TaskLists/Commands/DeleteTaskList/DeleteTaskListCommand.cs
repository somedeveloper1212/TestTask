using HelsiTestTask.Application.Mediator.Core;
using HelsiTestTask.Domain.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace HelsiTestTask.Application.Features.TaskLists.Commands.DeleteTaskList;

public class DeleteTaskListCommand: IRequest<Result>
{
    [FromHeader(Name = "X-User-Id")]
    public required string ClientId { get; init; }
    
    [FromRoute] 
    public required string Id { get; init; }
}
