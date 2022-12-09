namespace Validator;

public class AppSettings
{
    public int MaxConcurrent { get; set; }
    public string DataHandlerUrl { get; set; }
    public string FileStorageUrl { get; set; }
    public bool IgnoreMismatchedTypes { get; set; }
}