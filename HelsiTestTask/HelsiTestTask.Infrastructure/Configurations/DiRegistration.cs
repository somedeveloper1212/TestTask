using HelsiTestTask.Domain.TaskLists;
using HelsiTestTask.Infrastructure.Common;
using HelsiTestTask.Infrastructure.MongoDb.Entities;
using HelsiTestTask.Infrastructure.MongoDb.Indexes;
using HelsiTestTask.Infrastructure.MongoDb.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HelsiTestTask.Infrastructure.Configurations;

public static class DiRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddMongoCollection<TaskListEntity>(CollectionNames.TaskLists);
        services.AddScoped<ITaskListsRepository, TaskListsRepository>();
        services.AddHostedService<MongoIndexesHostedService>();

        return services;
    }
    
    public static IServiceCollection AddMongoClient(
        this IServiceCollection services,
        IConfiguration configuration,
        string configurationSection = "MongoDb")
    {
        services.Configure<MongoDbSettings>(configuration.GetSection(configurationSection));
        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            var mongoUrl = new MongoUrl(settings.ConnectionString);
            
            return new MongoClient(mongoUrl);
        });

        services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            var client = sp.GetRequiredService<IMongoClient>();

            return client.GetDatabase(settings.DatabaseName);
        });
        
        return services;
    }
    
    private static IServiceCollection AddMongoCollection<T>(
        this IServiceCollection services, string collectionName)
    {
        services.AddSingleton<IMongoCollection<T>>(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return db.GetCollection<T>(collectionName);
        });

        return services;
    }
}
