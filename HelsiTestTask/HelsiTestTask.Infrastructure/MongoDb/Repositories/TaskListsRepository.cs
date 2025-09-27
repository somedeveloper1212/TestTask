using HelsiTestTask.Domain.TaskLists;
using HelsiTestTask.Infrastructure.MongoDb.Entities;
using HelsiTestTask.Infrastructure.MongoDb.Mappers;
using MongoDB.Driver;

namespace HelsiTestTask.Infrastructure.MongoDb.Repositories;

public class TaskListsRepository: ITaskListsRepository
{
    private readonly IMongoCollection<TaskListEntity> _collection;

    public TaskListsRepository(IMongoCollection<TaskListEntity> collection)
    {
        _collection = collection;
    }

    public async Task<TaskList?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var entity = await _collection.Find(e => e.Id == id).FirstOrDefaultAsync(cancellationToken);
        
        return entity?.ToDomain();
    }
    
    public async Task<List<TaskList>> GetByUserIdAsync(
        string userId,
        bool descending,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        var filter = Builders<TaskListEntity>.Filter.Or(
            Builders<TaskListEntity>.Filter.Eq(e => e.CreatedById, userId),
            Builders<TaskListEntity>.Filter.AnyEq(e => e.SharedWithUsersIds, userId) 
        );
        
        var sort = descending 
            ? Builders<TaskListEntity>.Sort.Descending(e => e.CreatedAtUtc)
            : Builders<TaskListEntity>.Sort.Ascending(e => e.CreatedAtUtc);
        
        var entities = await _collection
            .Find(filter)
            .Sort(sort)
            .Skip(skip)
            .Limit(take)
            .ToListAsync(cancellationToken); 
        
        return entities.Select(e => e.ToDomain()).ToList();
    }
    
    public async Task CreateAsync(TaskList taskList, CancellationToken cancellationToken)
    {
        var entity = taskList.ToEntity();
        
        await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(TaskList taskList, CancellationToken cancellationToken)
    {
        var entity = taskList.ToEntity();
        var filter = Builders<TaskListEntity>.Filter.Eq(e => e.Id, entity.Id);
        
        await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<TaskListEntity>.Filter.Eq(e => e.Id, id);
        
        await _collection.DeleteOneAsync(filter, cancellationToken);
    }
}
