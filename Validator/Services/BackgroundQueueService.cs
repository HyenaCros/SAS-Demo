using System.Collections.Concurrent;
using Shared;

namespace Validator.Services;

public class BackgroundQueueService : BackgroundService
{
    private readonly AppSettings _appSettings;
    private readonly ValidationService _validationService;
    public static readonly ConcurrentQueue<Guid> Queue = new();

    public BackgroundQueueService(AppSettings appSettings, ValidationService validationService)
    {
        _appSettings = appSettings;
        _validationService = validationService;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var batch = new List<Guid>();
            while (Queue.TryDequeue(out var id) && batch.Count < _appSettings.MaxConcurrent)
            {
                batch.Add(id);
            }
            
            if (batch.Count > 0)
                await Task.WhenAll(batch.Select(x => _validationService.ValidateFileData(x)));

            await Task.Delay(1000, stoppingToken);
        }
    }
}