using Amazon.S3;
using Amazon.S3.Model;
using FileService.Core;
using FileService.Core.Documents;
using FileService.Endpoints;

namespace FileService.Features;

public static class CompleteMultipartUpload
{
    private record PartETagInfo(int PartNumber, string ETag);

    private record CompleteMultipartRequest(string UploadId, List<PartETagInfo> Parts);

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key:guid}/complete-multipart", Handler);
        }
    }

    private static async Task<IResult> Handler(
        Guid key,
        CompleteMultipartRequest request,
        IAmazonS3 s3Client,
        IFilesRepository filesRepository,
        CancellationToken cancellationToken)
    {
        try
        {
            var completeRequest = new CompleteMultipartUploadRequest
            {
                BucketName = "bucket",
                Key = $"videos/{key}",
                UploadId = request.UploadId,
                PartETags = request.Parts.Select(p => new PartETag(p.PartNumber, p.ETag)).ToList()
            };

            var response = await s3Client.CompleteMultipartUploadAsync(
                completeRequest,
                cancellationToken);

            var metaDataRequest = new GetObjectMetadataRequest
            {
                BucketName = "bucket",
                Key = response.Key
            };

            var metadata = await s3Client.GetObjectMetadataAsync(metaDataRequest, cancellationToken);

            var file = new FileDocument
            {
                StoragePath = response.Key,
                ContentType = metadata.Headers.ContentType,
                FileSize = metadata.ContentLength,
                UploadDate = DateTime.UtcNow
            };

            await filesRepository.Add(file, cancellationToken);

            return Results.Ok(new
            {
                key,
                location = response.Location
            });
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3 error complete multipart upload: {ex.Message}");
        }
    }
}