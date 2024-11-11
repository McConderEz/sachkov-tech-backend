using System.Net.Http.Json;
using FileService.Contracts;

namespace FileService.Communication;

public class FileHttpClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<FileResponse>> GetFilesByIdsAsync(GetFilesByIdsRequest request, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync("files", request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var files = await response.Content.ReadFromJsonAsync<IEnumerable<FileResponse>>(cancellationToken);

        return files?.ToList() ?? [];
    }
}