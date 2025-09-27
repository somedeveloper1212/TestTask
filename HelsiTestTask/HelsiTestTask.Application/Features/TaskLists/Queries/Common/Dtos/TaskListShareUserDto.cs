namespace HelsiTestTask.Application.Features.TaskLists.Queries.Common.Dtos;

public class TaskListShareUserDto
{
    public required string UserId { get; init; }
    public required bool IsOwner { get; init; }
}
