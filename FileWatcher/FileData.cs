using Shared;

namespace FileWatcher;

public class FileData : BaseFileData
{
    public int RecordCount { get; set; }
    public string Checksum { get; set; }
    public string Contents { get; set; }
}