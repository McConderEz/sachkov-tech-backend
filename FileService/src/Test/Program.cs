using FileService.Communication;
using FileService.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFileHttpClient(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/fileservice", async (FileHttpClient fileHttpClient, CancellationToken cancellationToken) =>
{
    var files = await fileHttpClient.GetFilesByIdsAsync(
        new GetFilesByIdsRequest([Guid.Parse("2572d9ad-a013-4645-be3e-b79dbfcd4c09")]),
        cancellationToken);

    return files;
});

app.Run();