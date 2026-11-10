using HelsiTestTask.Infrastructure.MongoDb.Entities;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace HelsiTestTask.Infrastructure.MongoDb.Indexes;

/// <summary>
/// Creates the MongoDB indexes on application startup, before the app starts serving requests.
/// Creating an index that already exists with the same keys and options is a no-op.
/// </summary>
public class MongoIndexesInitializer : IHostedService
{
    private readonly IMongoCollection<TaskListEntity> _taskLists;

    public MongoIndexesInitializer(IMongoCollection<TaskListEntity> taskLists)
    {
        _taskLists = taskLists;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _taskLists.Indexes.CreateManyAsync(TaskListIndexes.All, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
