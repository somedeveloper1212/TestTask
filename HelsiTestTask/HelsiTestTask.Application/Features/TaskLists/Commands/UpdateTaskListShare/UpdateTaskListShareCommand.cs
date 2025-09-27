using HelsiTestTask.Application.Mediator.Core;
using HelsiTestTask.Domain.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace HelsiTestTask.Application.Features.TaskLists.Commands.UpdateTaskListShare;

public class UpdateTaskListShareCommand: IRequest<Result>
{
    [FromRoute] 
    public required string Id { get; init; }
    
    [FromRoute] 
    public required string UserId { get; init; }

    /// <summary>
    /// true = Grant access, false = Revoke access
    /// </summary>
    [FromBody] 
    public required bool Grant { get; init; }

    [FromHeader(Name = "X-User-Id")]
    public required string RequestedById { get; init; }
}
