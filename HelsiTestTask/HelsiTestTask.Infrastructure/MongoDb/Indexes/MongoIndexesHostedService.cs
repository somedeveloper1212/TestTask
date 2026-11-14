using HelsiTestTask.Infrastructure.MongoDb.Entities;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace HelsiTestTask.Infrastructure.MongoDb.Indexes;

public class MongoIndexesHostedService : IHostedService
{
    private readonly IMongoCollection<TaskListEntity> _taskLists;

    public MongoIndexesHostedService(IMongoCollection<TaskListEntity> taskLists)
    {
        _taskLists = taskLists;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _taskLists.Indexes.CreateManyAsync(TaskListIndexes.All, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
