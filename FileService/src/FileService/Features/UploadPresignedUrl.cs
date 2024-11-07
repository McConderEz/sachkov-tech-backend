using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;

namespace FileService.Features;

public static class UploadPresignedUrl
{
    private record UploadPresignedUrlRequest(string FileName, string ContentType);

    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/presigned", Handler);
        }
    }

    private static async Task<IResult> Handler(UploadPresignedUrlRequest uploadPresignedUrlRequest, IAmazonS3 s3Client)
    {
        try
        {
            var key = Guid.NewGuid();

            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = "file-service",
                Key = $"videos/{key}",
                Verb = HttpVerb.PUT,
                Expires = DateTime.Now.AddMinutes(15),
                ContentType = uploadPresignedUrlRequest.ContentType,
                Protocol = Protocol.HTTP,
                Metadata =
                {
                    ["file-name"] = uploadPresignedUrlRequest.FileName
                }
            };

            var presignedUrl = await s3Client.GetPreSignedURLAsync(presignedRequest);

            return Results.Ok(new
            {
                key,
                url = presignedUrl
            });
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3 error generating presigned URL: {ex.Message}");
        }
    }
}