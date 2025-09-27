namespace HelsiTestTask.Application.Features.TaskLists.Queries.Common.Dtos;

public class TaskListDto
{
    public required string Id { get; init; }

    public required string Name { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    
    public DateTime? UpdatedAt { get; init; }
    
    public required bool IsOwner { get; init; }
}
