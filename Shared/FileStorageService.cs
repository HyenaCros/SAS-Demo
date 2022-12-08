using System.Web;

namespace Shared;

public class FileStorageService
{
    private readonly HttpClient _httpClient;

    public FileStorageService(string fileStorageUrl)
    {
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(fileStorageUrl)
        };
    }

    public async Task SaveFile(string fileName, string fileContents)
    {
        var content = new MultipartFormDataContent()
        {
            {new StringContent(fileContents), "File", fileName}
        };
        var response = await _httpClient.PostAsync($"api/File", content);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode);
    }

    public async Task<string> GetFileContents(string fileName)
    {
        var response = await _httpClient.GetAsync($"api/File/{HttpUtility.UrlEncode(fileName)}");
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode);
        return await response.Content.ReadAsStringAsync();
    }
}