using HelsiTestTask.Application.Features.TaskLists.Queries.Common.Dtos;
using HelsiTestTask.Domain.TaskLists;

namespace HelsiTestTask.Application.Features.TaskLists.Queries.Common.Mappings;

public static class TaskListMapping
{
    public static TaskListDto ToDto(this TaskList entity, string userId)
    {
        return new TaskListDto
        {
            Id = entity.Id,
            Name = entity.Name,
            CreatedAt = entity.CreatedAtUtc,
            UpdatedAt = entity.UpdatedAtUtc,
            IsOwner = entity.CreatedById.Equals(userId, StringComparison.OrdinalIgnoreCase)
        };
    }
}
