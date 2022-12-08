using Validator.Models;

namespace Shared;

public class AddClaimRecordsRequest
{
    public List<ClaimRecord> ClaimRecords { get; set; }
}