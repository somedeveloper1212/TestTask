using HelsiTestTask.Domain.TaskLists;
using HelsiTestTask.Infrastructure.MongoDb.Entities;

namespace HelsiTestTask.Infrastructure.MongoDb.Mappers;

public static class TaskListMapper
{
    public static TaskList ToDomain(this TaskListEntity entity)
    {
        return new TaskList
        {
            Id = entity.Id,
            Name = entity.Name,
            SharedWithUsersIds = entity.SharedWithUsersIds,
            CreatedById = entity.CreatedById,
            CreatedAtUtc = entity.CreatedAtUtc,
            UpdatedAtUtc = entity.UpdatedAtUtc,
        };
    }

    public static TaskListEntity ToEntity(this TaskList domain)
    {
        return new TaskListEntity
        {
            Id = domain.Id,
            Name = domain.Name,
            SharedWithUsersIds = domain.SharedWithUsersIds,
            CreatedById = domain.CreatedById,
            CreatedAtUtc = domain.CreatedAtUtc,
            UpdatedAtUtc = domain.UpdatedAtUtc
        };
    }
}
