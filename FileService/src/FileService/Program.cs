using Amazon.S3;
using FileService;
using FileService.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddMinio(builder.Configuration);

builder.Services.AddSingleton<IAmazonS3>(_ =>
{
    var config = new AmazonS3Config
    {
        ServiceURL = "http://localhost:9000",
        ForcePathStyle = true,
        UseHttp = true
    };

    return new AmazonS3Client("minioadmin", "minioadmin", config);
});

builder.Services.AddEndpoints();

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapEndpoints();

app.Run();