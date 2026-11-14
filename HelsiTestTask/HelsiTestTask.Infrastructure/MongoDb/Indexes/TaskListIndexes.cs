using HelsiTestTask.Infrastructure.MongoDb.Entities;
using MongoDB.Driver;

namespace HelsiTestTask.Infrastructure.MongoDb.Indexes;

public static class TaskListIndexes
{
    public const string CreatedByIdCreatedAtUtc = "ix_created_by_id_created_at_utc";
    public const string SharedWithUsersIdsCreatedAtUtc = "ix_shared_with_users_ids_created_at_utc";

    private static readonly IndexKeysDefinitionBuilder<TaskListEntity> Keys = Builders<TaskListEntity>.IndexKeys;

    // GetByUserIdAsync filters by "CreatedById == userId OR SharedWithUsersIds contains userId"
    // and sorts by CreatedAtUtc. With an index per $or branch that ends with the sort key,
    // MongoDB scans each branch in sort order and merges them (SORT_MERGE), so skip/limit
    // paging needs neither a collection scan nor an in-memory sort.
    // Both directions of the sort are served, because an index can be walked backwards.
    public static IReadOnlyList<CreateIndexModel<TaskListEntity>> All { get; } =
    [
        // Lists the user owns.
        new(
            Keys.Ascending(e => e.CreatedById).Descending(e => e.CreatedAtUtc),
            new CreateIndexOptions { Name = CreatedByIdCreatedAtUtc }),

        // Lists shared with the user (multikey index over the array of user ids).
        new(
            Keys.Ascending(e => e.SharedWithUsersIds).Descending(e => e.CreatedAtUtc),
            new CreateIndexOptions { Name = SharedWithUsersIdsCreatedAtUtc }),
    ];
}
