using Microsoft.OpenApi.Models;
using SachkovTech.Accounts.Infrastructure.Seeding;
using SachkovTech.Core.Extensions;
using SachkovTech.Framework.Middlewares;
using SachkovTech.Web;
using SachkovTech.Web.Middlewares;
using Serilog;


DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

builder.Services.AddLogging(builder.Configuration);

builder.Services.AddFramework();
builder.Services.AddAccountsModule(builder.Configuration);
builder.Services.AddFilesModule(builder.Configuration);
builder.Services.AddIssuesModule(builder.Configuration);
builder.Services.AddApplicationLayers();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthServices(builder.Configuration);

var app = builder.Build();

app.Services.RunMigrations();

var accountsSeeder = app.Services.GetRequiredService<AccountsSeeder>();

await accountsSeeder.SeedAsync();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(config =>
{
    config.WithOrigins("http://localhost:5173")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseAuthentication();
app.UseAuthorization();
app.UseScopeDataMiddleware();

app.MapControllers();

app.Run();