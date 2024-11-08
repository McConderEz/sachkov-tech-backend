using Amazon.S3;
using Amazon.S3.Model;
using FileService.Contracts;
using FileService.Core;
using FileService.Endpoints;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace FileService.Features;

public static class GetFilesByIds
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files", Handler);
        }
    }

    private static async Task<IResult> Handler(
        GetFilesByIdsRequest request,
        IFilesRepository filesRepository,
        IAmazonS3 s3Client,
        CancellationToken cancellationToken)
    {
        var files = await filesRepository.Get(request.Ids, cancellationToken);

        var presignedUrls = new List<FileResponse>();

        foreach (var file in files)
        {
            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = "bucket",
                Key = file.StoragePath,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddHours(24),
                Protocol = Protocol.HTTP
            };

            var presignedUrl = await s3Client.GetPreSignedURLAsync(presignedRequest);
            presignedUrls.Add(new FileResponse(file.Id, presignedUrl));
        }

        return Results.Ok(presignedUrls);
    }
}