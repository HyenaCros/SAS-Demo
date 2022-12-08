namespace FileWatcher;

public class AppSettings
{
    public FtpSettings Ftp { get; set; }
    public string FileStorageUrl { get; set; }
    public string ValidatorUrl { get; set; }
    public string DataHandlerUrl { get; set; }
    public bool DeleteFiles { get; set; }
}

public class FtpSettings
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
    public string Path { get; set; }
}