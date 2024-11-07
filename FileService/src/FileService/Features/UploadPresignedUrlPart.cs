using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;

namespace FileService.Features;

public static class UploadPresignedUrlPart
{
    private record UploadPresignedUrlPartRequest(string UploadId, int PartNumber);

    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key}/presigned-part", Handler);
        }
    }

    private static async Task<IResult> Handler(string key, UploadPresignedUrlPartRequest uploadPresignedUrlPartRequest, IAmazonS3 s3Client)
    {
        try
        {
            var presignedPartRequest = new GetPreSignedUrlRequest
            {
                BucketName = "file-service",
                Key = $"videos/{key}",
                Verb = HttpVerb.PUT,
                Expires = DateTime.Now.AddMinutes(15),
                Protocol = Protocol.HTTP,
                UploadId = uploadPresignedUrlPartRequest.UploadId,
                PartNumber = uploadPresignedUrlPartRequest.PartNumber
            };

            var presignedUrl = await s3Client.GetPreSignedURLAsync(presignedPartRequest);

            return Results.Ok(new
            {
                key,
                url = presignedUrl
            });
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3 error generating presigned URL for part: {ex.Message}");
        }
    }
}