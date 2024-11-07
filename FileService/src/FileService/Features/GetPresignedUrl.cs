﻿using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;

namespace FileService.Features;

public static class GetPresignedUrl
{
    public sealed class EndPoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("files/{key}/presigned", Handler);
        }
    }

    private static async Task<IResult> Handler(string key, IAmazonS3 s3Client)
    {
        try
        {
            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = "file-service",
                Key = key,
                Verb = HttpVerb.GET,
                Expires = DateTime.Now.AddHours(24),
                Protocol = Protocol.HTTP,
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