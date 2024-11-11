namespace FileService.Contracts;

public record GetFilesByIdsRequest(Guid[] Ids);

public record FileResponse(Guid Id, string Url);