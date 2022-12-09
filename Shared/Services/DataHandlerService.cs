using System.Net.Http.Json;
using System.Text.Json;
using Shared.Dtos;
using Validator.Models;

namespace Shared;

public class DataHandlerService
{
    private readonly HttpClient _httpClient;
    
    public DataHandlerService(string dataHandlerUrl)
    {
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(dataHandlerUrl)
        };
    }
    public async Task<List<Guid>> AddFileUploads(AddFileUploadsRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Uploads/Bulk", request);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode);
        return await response.Content.ReadFromJsonAsync<List<Guid>>();
    }

    public async Task UpdateFileUpload(ValidationResult result)
    {
        var response = await _httpClient.PutAsJsonAsync("api/Uploads", result);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode);
    }

    public async Task<List<ErrorLogRecord>> GetErrorRecords()
    {
        return await _httpClient.GetFromJsonAsync<List<ErrorLogRecord>>("api/Uploads/Errors");
    }

    public async Task<FileUploadRecord> GetFileUpload(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<FileUploadRecord>($"api/Uploads/{id}");
    }

    public async Task<List<FileUploadRecord>> GetFileUploads()
    {
        return await _httpClient.GetFromJsonAsync<List<FileUploadRecord>>("api/Uploads");
    }
}