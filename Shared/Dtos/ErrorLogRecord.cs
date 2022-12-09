namespace Shared.Dtos;

public class ErrorLogRecord : ErrorRecord
{
    public Guid Id { get; set; }
    public Guid FileUploadId { get; set; }
    public string FileName { get; set; }
    public DateTime DateCreated { get; set; }
}