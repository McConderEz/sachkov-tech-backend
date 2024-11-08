using FileService.Core.Documents;
using MongoDB.Driver;

namespace FileService.MongoDataAccess;

public class MongoDbContext(IMongoClient mongoClient)
{
    private readonly IMongoDatabase _database = mongoClient.GetDatabase("file_service");

    public IMongoCollection<FileDocument> Files => _database.GetCollection<FileDocument>("files");
}