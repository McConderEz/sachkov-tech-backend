using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;
using Hangfire;

namespace FileService.Features;

public static class ApplyUpload
{
    private record ApplyUploadRequest(string key, string FileName, string ContentType);

    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/multipart", Handler);
        }
    }

    private static async Task<IResult> Handler(ApplyUploadRequest applyUploadRequest, IAmazonS3 s3Client, BackgroundJobClient client)
    {
        var jobId = client.Schedule(() => Console.WriteLine(), TimeSpan.Zero);
        
        client.
        try
        {
            // проверить, что файл существует по ключу, убрать задачу 
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3 error starting multipart upload: {ex.Message}");
        }
    }
}