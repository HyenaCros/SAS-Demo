using System.Collections.Concurrent;
using Shared;

namespace Validator.Services;

public class BackgroundQueueService : BackgroundService
{
    private readonly ImportService _importService;
    private readonly AppSettings _appSettings;
    public static readonly ConcurrentQueue<BaseFileData> Queue = new();

    public BackgroundQueueService(ImportService importService, AppSettings appSettings)
    {
        _importService = importService;
        _appSettings = appSettings;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var batch = new List<BaseFileData>();
            while (Queue.TryDequeue(out var fileData) && batch.Count < _appSettings.MaxConcurrent)
            {
                batch.Add(fileData);
            }
            
            if (batch.Count > 0)
                await Task.WhenAll(batch.Select(x => _importService.ImportFileData(x)));

            await Task.Delay(1000, stoppingToken);
        }
    }
}