using System.Net.Http.Json;

namespace Shared.Services;

public class ValidationService
{
    private readonly HttpClient _httpClient;
    public ValidationService(string validatorUrl)
    {
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(validatorUrl)
        };
    }

    public async Task ValidateFiles(List<Guid> files)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Validation", new ValidateFilesRequest()
        {
            Files = files
        });

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode);
    }
}