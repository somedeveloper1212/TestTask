using HelsiTestTask.Domain.Common.Result;

namespace HelsiTestTask.Domain.TaskLists;

public static class TaskListErrors
{
    public static NotFound TaskListNotFound(string taskListId) =>
        new(nameof(TaskListNotFound), $"TaskList with id {taskListId} not found.");

    public static Forbidden TaskListAccessDenied =>
        new(nameof(TaskListAccessDenied), "You don't have permission to this TaskList.");
    
    public static Conflict CannotShareWithOwner(string userId, string taskListId) =>
        new(nameof(CannotShareWithOwner),
            $"User {userId} is the owner of TaskList {taskListId} and cannot be added or removed as a shared user.");
    
    public static Forbidden OnlyOwnerCanDoThis =>
        new(nameof(OnlyOwnerCanDoThis), "This action can only be performed by the TaskList owner.");
}
