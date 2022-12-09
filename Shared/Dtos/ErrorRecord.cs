namespace Shared;

public class ErrorRecord
{
    public string ErrorMessage { get; set; }
    public int? LineNumber { get; set; }
    public int? ColumnNumber { get; set; }
    public string Line { get; set; }
}