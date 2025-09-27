namespace HelsiTestTask.Domain.TaskLists;

public interface ITaskListsRepository
{
    Task<TaskList?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<List<TaskList>> GetByUserIdAsync(
        string userId,
        bool descending,
        int skip,
        int take,
        CancellationToken cancellationToken);
    Task CreateAsync(TaskList taskList, CancellationToken cancellationToken);
    Task UpdateAsync(TaskList taskList, CancellationToken cancellationToken);
    Task DeleteAsync(string id, CancellationToken cancellationToken);
}