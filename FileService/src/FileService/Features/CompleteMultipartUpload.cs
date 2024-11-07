using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;

namespace FileService.Features;

public static class CompleteMultipartUpload
{
    private record PartETagInfo(int PartNumber, string ETag);
    private record CompleteMultiPartUpload(string UploadId, List<PartETagInfo> Parts);

    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/{key}/complete-multipart", Handler);
        }
    }

    private static async Task<IResult> Handler(string key, CompleteMultiPartUpload completeMultiPartUpload, IAmazonS3 s3Client)
    {
        try
        {
            var completeRequest = new CompleteMultipartUploadRequest
            {
                BucketName = "file-service",
                Key = $"videos/{key}",
                UploadId = completeMultiPartUpload.UploadId,
                PartETags = completeMultiPartUpload.Parts.Select(p => new PartETag(p.PartNumber, p.ETag)).ToList()
            };

            var response = await s3Client.CompleteMultipartUploadAsync(completeRequest);

            return Results.Ok(new
            {
                key,
                location = response.Location
            });
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3 error generating presigned URL for part: {ex.Message}");
        }
    }
}