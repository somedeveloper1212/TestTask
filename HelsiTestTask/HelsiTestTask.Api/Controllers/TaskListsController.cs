using HelsiTestTask.Api.Common.Extensions;
using HelsiTestTask.Api.Common.Responses;
using HelsiTestTask.Application.Features.TaskLists.Commands.CreateTaskList;
using HelsiTestTask.Application.Features.TaskLists.Commands.DeleteTaskList;
using HelsiTestTask.Application.Features.TaskLists.Commands.UpdateTaskList;
using HelsiTestTask.Application.Features.TaskLists.Commands.UpdateTaskListShare;
using HelsiTestTask.Application.Features.TaskLists.Queries.Common.Dtos;
using HelsiTestTask.Application.Features.TaskLists.Queries.GetTaskListById;
using HelsiTestTask.Application.Features.TaskLists.Queries.GetTaskLists;
using HelsiTestTask.Application.Features.TaskLists.Queries.GetTaskListShares;
using HelsiTestTask.Application.Mediator.Core;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HelsiTestTask.Api.Controllers;

[Route("api/task-lists")]
[SwaggerTag("TaskLists")]
public class TaskListsController: HelsiTestTaskController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskListsController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator for handling commands and queries.</param>
    public TaskListsController(IMediator mediator) : base(mediator)
    {
    }
    
    /// <summary>
    /// Creates a new tasklist for the authenticated user.
    /// </summary>
    /// <param name="command">The data for the tasklist to be created.</param>
    /// <param name="cancellationToken">Token for request cancellation.</param>
    /// <returns>The newly created tasklist's ID.</returns>
    /// <response code="201">Returns the newly created tasklist's ID.</response>
    /// <response code="400">If validation fails or input is incorrect.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CreateTaskListCommandResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Create a tasklist",
        Description = "Creates a new tasklist for the current user.",
        OperationId = "CreateTasklist"
    )]
    public async Task<IActionResult> CreateTasklist(CreateTaskListCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return result.ToActionResult(r =>
            CreatedAtAction(nameof(CreateTasklist),
                new { id = result.ValueOrDefault.Id },
                r.MapToApiResponse()));
    }
    
    /// <summary>
    /// Updates an existing tasklist by ID.
    /// </summary>
    /// <param name="command">The command containing the tasklist ID, updated data, and user ID.</param>
    ///<param name="cancellationToken">Token for request cancellation.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">If the tasklist was successfully updated.</response>
    /// <response code="400">If validation fails.</response>
    /// <response code="404">If the tasklist was not found.</response>
    [HttpPut("{Id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Update tasklist",
        Description = "Updates an existing tasklist by ID.",
        OperationId = "UpdateTasklist"
    )]
    public async Task<IActionResult> UpdateTasklist(UpdateTaskListCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return result.ToActionResult(_ => NoContent());
    }
    
    /// <summary>
    /// Deletes a specific tasklist by ID.
    /// </summary>
    /// <param name="command">The command containing user ID and the tasklist ID to delete.</param>
    /// <param name="cancellationToken">Token for request cancellation.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">If the tasklist was successfully deleted.</response>
    /// <response code="404">If the tasklist was not found.</response>
    [HttpDelete("{Id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Delete tasklist",
        Description = "Deletes the specified tasklist by ID.",
        OperationId = "DeleteTasklist"
    )]
    public async Task<IActionResult> DeleteTasklist(DeleteTaskListCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return result.ToActionResult(_ => NoContent());
    }
    
    /// <summary>
    /// Retrieves a specific tasklist by ID.
    /// </summary>
    /// <param name="query">The query object containing the tasklist ID and user ID.</param>
    /// <param name="cancellationToken">Token for request cancellation.</param>
    /// <returns>The tasklist with the specified ID.</returns>
    /// <response code="200">Returns the tasklist.</response>
    /// <response code="404">If the tasklist was not found.</response>
    [HttpGet("{Id}")]
    [ProducesResponseType(typeof(ApiResponse<TaskListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get tasklist by ID",
        Description = "Returns a single tasklist by its ID.",
        OperationId = "GetTasklistById"
    )]
    public async Task<IActionResult> GetTasklistById(GetTaskListByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(query, cancellationToken);
        return result.ToActionResult(r => Ok(r.MapToApiResponse()));
    }
    
    /// <summary>
    /// Retrieves a paginated list of tasklists for the current user.
    /// </summary>
    /// <param name="query">The query object containing user ID, pagination things and sorting.</param>
    /// <param name="cancellationToken">Token for request cancellation.</param>
    /// <returns>A paginated list of tasklists.</returns>
    /// <response code="200">Returns the paginated list of tasklists.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<TaskListDto>>), StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "Get tasklists",
        Description = "Returns a paginated list of tasklists for the current user.",
        OperationId = "GetTasklists"
    )]
    public async Task<IActionResult> GetTasklists(GetTaskListsQuery query, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(query, cancellationToken);
        return result.ToActionResult(r => Ok(r.MapToApiResponse()));
    }

    /// <summary>
    /// Grants or revokes access to a tasklist for a specific user.
    /// </summary>
    /// <param name="command">The command containing tasklist ID, target user ID, and Grant flag.</param>
    /// <param name="cancellationToken">Token for request cancellation.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">If access was successfully updated.</response>
    /// <response code="400">If validation fails.</response>
    /// <response code="404">If the tasklist or user was not found.</response>
    [HttpPost("{Id}/share/{UserId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Update tasklist sharing",
        Description = "Grants or revokes access to a tasklist for a specific user. Use Grant=true to add, Grant=false to remove.",
        OperationId = "UpdateTasklistShare"
    )]
    public async Task<IActionResult> UpdateTasklistShare(
        UpdateTaskListShareCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return result.ToActionResult(_ => NoContent());
    }
    
    /// <summary>
    /// Retrieves all users who have access to a specific tasklist.
    /// </summary>
    /// <param name="query">The query containing the tasklist ID.</param>
    /// <param name="cancellationToken">Token for request cancellation.</param>
    /// <returns>A list of users with access to the tasklist.</returns>
    /// <response code="200">Returns the list of users with access.</response>
    /// <response code="404">If the tasklist was not found.</response>
    [HttpGet("{Id}/share")]
    [ProducesResponseType(typeof(ApiResponse<List<TaskListShareUserDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get tasklist shares",
        Description = "Returns all users who have access to the specified tasklist.",
        OperationId = "GetTasklistShares"
    )]
    public async Task<IActionResult> GetTasklistShares(
        GetTaskListSharesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(query, cancellationToken);
        return result.ToActionResult(r => Ok(r.MapToApiResponse()));
    }
}
