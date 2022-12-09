namespace Shared.Dtos;

public class BaseFileUpload
{
    public DateTime DateUploaded { get; set; } = DateTime.Now;
    public string FileName { get; set; }
    public ClaimType? Type { get; set; }
    public FileSource Source { get; set; }
    public List<ErrorRecord> Errors { get; set; } = new();
}