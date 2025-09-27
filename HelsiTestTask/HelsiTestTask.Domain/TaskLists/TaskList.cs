using HelsiTestTask.Domain.Common.Result;

namespace HelsiTestTask.Domain.TaskLists;

public class TaskList
{
    public required string Id { get; init; }

    public required string Name { get; set; }

    public HashSet<string> SharedWithUsersIds { get; set; } = [];
    
    public required string CreatedById { get; init; }

    public required DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAtUtc { get; set; }
    
    public static Result<TaskList> TryCreate(
        string id,
        string name,
        string createdByUserId
    )
    {
        return new TaskList
        {
            Id = id,
            Name = name,
            CreatedById = createdByUserId,
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}
