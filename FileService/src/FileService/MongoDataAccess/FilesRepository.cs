using CSharpFunctionalExtensions;
using FileService.Core;
using FileService.Core.Documents;
using FileService.Core.Models;
using MongoDB.Driver;

namespace FileService.MongoDataAccess;

public class FilesRepository : IFilesRepository
{
    private readonly MongoDbContext _dbContext;
    private readonly ILogger<FilesRepository> _logger;
    public FilesRepository(MongoDbContext dbContext, ILogger<FilesRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Add(FileDocument file, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Files.InsertOneAsync(file, cancellationToken: cancellationToken);

            return file.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to save file in database.");

            return Errors.Files.FailUpload();
        }
    }

    public async Task<IEnumerable<FileDocument>> Get(IEnumerable<Guid> fileIds, CancellationToken cancellationToken) =>
        await _dbContext.Files.Find(f => fileIds.Contains(f.Id)).ToListAsync(cancellationToken);

    public async Task<UnitResult<Error>> Remove(IEnumerable<Guid> fileIds, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dbContext.Files.DeleteManyAsync(f => fileIds.Contains(f.Id), cancellationToken);

            if (result.DeletedCount == 0)
                return Errors.Files.FailRemove();

            return Result.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to remove file from database.");

            return Errors.Files.FailRemove();
        }
    }
}