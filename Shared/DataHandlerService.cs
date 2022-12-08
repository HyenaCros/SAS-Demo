using System.Net.Http.Json;
using System.Text.Json;
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
    public async Task AddErrorRecords(List<ErrorRecord> errors)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Errors", new AddErrorRecordsRequest()
        {
            ErrorRecords = errors
        });
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode);
    }

    public async Task AddClaimRecords(List<ClaimRecord> claims)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Claims", new AddClaimRecordsRequest()
        {
            ClaimRecords = claims
        });
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode);
    }

    public async Task<List<ErrorRecord>> GetErrorRecords()
    {
        return await _httpClient.GetFromJsonAsync<List<ErrorRecord>>("api/Errors");
    }
}