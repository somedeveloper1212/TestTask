using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HelsiTestTask.Infrastructure.MongoDb.Entities;

public class TaskListEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public required string Id { get; set; }
    
    [MaxLength(255)]
    public required string Name { get; set; }
    
    public HashSet<string> SharedWithUsersIds { get; set; } = [];
    
    public required string CreatedById { get; set; }

    public required DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAtUtc { get; set; }
}

