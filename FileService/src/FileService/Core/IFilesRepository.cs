using CSharpFunctionalExtensions;
using FileService.Core.Documents;
using FileService.Core.Models;

namespace FileService.Core;

public interface IFilesRepository
{
    Task<Result<Guid, Error>> Add(FileDocument file, CancellationToken cancellationToken);
    Task<IEnumerable<FileDocument>> Get(IEnumerable<Guid> fileIds, CancellationToken cancellationToken);
    Task<UnitResult<Error>> Remove(IEnumerable<Guid> fileIds, CancellationToken cancellationToken);
}