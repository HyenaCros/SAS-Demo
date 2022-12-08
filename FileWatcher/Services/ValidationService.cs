using Shared;

namespace FileWatcher.Services;

public class ValidationService
{
    private readonly HttpClient _httpClient;
    public ValidationService(AppSettings appSettings)
    {
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(appSettings.ValidatorUrl)
        };
    }

    public async Task ValidateFiles(List<FileData> files)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Validation", new ValidateFilesRequest()
        {
            Files = files.Select(x => (BaseFileData)x).ToList()
        });
        Console.WriteLine(response.StatusCode.ToString());
        Console.WriteLine(response.RequestMessage.RequestUri.ToString());
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode);
    }
}