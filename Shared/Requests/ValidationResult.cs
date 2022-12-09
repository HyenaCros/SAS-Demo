using Validator.Models;

namespace Shared;

public class ValidationResult
{
    public Guid Id { get; set; }
    public List<ClaimRecord> ClaimRecords { get; set; }
    public List<ErrorRecord> ErrorRecords { get; set; }
}