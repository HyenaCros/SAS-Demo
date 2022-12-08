namespace Shared;

public class ErrorRecord
{
    public string FileName { get; set; }
    public string ErrorMessage { get; set; }
    public int? LineNumber { get; set; }
    public int? ColumnNumber { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public string Line { get; set; }
}