using HelsiTestTask.Application.Features.TaskLists.Queries.Common.Dtos;
using HelsiTestTask.Domain.TaskLists;

namespace HelsiTestTask.Application.Features.TaskLists.Queries.Common.Mappings;

public static class TaskListSharesMapping
{
    public static List<TaskListShareUserDto> ToShareDtos(this TaskList entity)
    {
        var result = new List<TaskListShareUserDto>();

        // Owner
        result.Add(new TaskListShareUserDto
        {
            UserId = entity.CreatedById,
            IsOwner = true
        });

        // Shared users
        result.AddRange(entity.SharedWithUsersIds.Select(id => new TaskListShareUserDto
        {
            UserId = id,
            IsOwner = false
        }));

        return result;
    }
}
