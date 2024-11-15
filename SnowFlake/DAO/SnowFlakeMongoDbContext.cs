
using MongoDB.Driver;

public class SnowFlakeMongoDbContext
{
    private readonly IMongoDatabase _database;

    public SnowFlakeMongoDbContext(IMongoClient mongoClient, IConfiguration configuration)
    {
        var databaseName = configuration.GetValue<string>("MongoDbSettings:DatabaseName");
        _database = mongoClient.GetDatabase(databaseName);
    }

    // Add methods to access collections and perform database operations here.
}