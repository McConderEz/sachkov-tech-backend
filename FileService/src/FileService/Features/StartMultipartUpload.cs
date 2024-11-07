using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;

namespace FileService.Features;

public static class StartMultipartUpload
{
    private record StartMultipartUploadRequest(string FileName, string ContentType);

    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/multipart", Handler);
        }
    }

    private static async Task<IResult> Handler(StartMultipartUploadRequest startMultipartUploadRequest, IAmazonS3 s3Client)
    {
        try
        {
            var key = Guid.NewGuid();

            var multipartRequest = new InitiateMultipartUploadRequest
            {
                BucketName = "file-service",
                Key = $"videos/{key}",
                ContentType = startMultipartUploadRequest.ContentType,
                Metadata =
                {
                    ["file-name"] = startMultipartUploadRequest.FileName
                }
            };

            var response = await s3Client.InitiateMultipartUploadAsync(multipartRequest);

            return Results.Ok(new
            {
                key,
                uploadId = response.UploadId
            });
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3 error starting multipart upload: {ex.Message}");
        }
    }
}