using Shared;

namespace FileWatcher;

public class FileData
{
    public string FileName { get; set; }
    public ClaimType FileType { get; set; }
    public int RecordCount { get; set; }
    public string Checksum { get; set; }
    public string Contents { get; set; }
}