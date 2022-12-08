using Rebex.Net;

namespace FileWatcher.Services;

public class BackgroundPollingService : BackgroundService
{
    private readonly ILogger<BackgroundPollingService> _logger;
    public IServiceProvider Services { get; }
    public static bool Enabled { get; private set; }

    public BackgroundPollingService(IServiceProvider serviceProvider, ILogger<BackgroundPollingService> logger)
    {
        _logger = logger;
        Services = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = Services.CreateAsyncScope();
        var config = scope.ServiceProvider.GetRequiredService<AppSettings>().Ftp;
        var ftpService = scope.ServiceProvider.GetRequiredService<FtpService>();
        var ftpClient = scope.ServiceProvider.GetRequiredService<Sftp>();
        await ftpClient.ConnectAsync(config.Host, config.Port);
        await ftpClient.LoginAsync(config.Username, config.Password);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            if (Enabled)
            {
                try
                {
                    await ftpService.PollTriggerFiles();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            }
            else
            {
                await Task.Delay(1000);
            }
        }
    }

    public static void Start()
    {
        Enabled = true;
    }

    public static void Stop()
    {
        Enabled = false;
    }
}