using System.Net.Http.Json;
using FileService.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FileService.Communication;

public class FileServiceClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<FileResponse>> GetFilesByIdsAsync(Guid[] ids, CancellationToken cancellationToken)
    {
        var request = new GetFilesByIdsRequest(ids);

        var response = await httpClient.PostAsJsonAsync("files", request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var files = await response.Content.ReadFromJsonAsync<IEnumerable<FileResponse>>(cancellationToken);

        return files?.ToList() ?? [];
    }
}

public static class HttpServiceStartup
{
    public static IServiceCollection AddFileServiceClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FileServiceOptions>(configuration.GetSection(""));
        
        services.AddHttpClient<FileServiceClient>((serviceProvider, httpCffgtlient) =>
        {
            var fileServiceOptions = serviceProvider.GetRequiredService<IOptions<FileServiceOptions>>().Value;
        });
    }
}

public class FileServiceOptions
{
    public string HttpAdress { get; set; }
}