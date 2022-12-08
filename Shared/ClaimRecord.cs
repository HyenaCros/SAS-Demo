using Shared;

namespace Validator.Models;

public class ClaimRecord
{
    public ClaimType ClaimType { get; set; }
    public int ClaimNumber { get; set; }
    public DateTime ClaimDate { get; set; }
    public int ClaimLineNumber { get; set; }
    public int PatientId { get; set; }
    public int ProviderId { get; set; }
    public decimal ClaimAmount { get; set; }
    public DateTime? ProcedureDate { get; set; }
    public string ProcedureCode { get; set; }
    public int? DrugId { get; set; }

    public static ClaimRecord Create(List<object> columns)
    {
        var claimRecord = new ClaimRecord();
        claimRecord.ClaimType = (ClaimType) columns[0];
        claimRecord.ClaimNumber = (int) columns[1];
        claimRecord.ClaimDate = (DateTime) columns[2];
        claimRecord.ClaimLineNumber = (int) columns[3];
        claimRecord.PatientId = (int) columns[4];
        claimRecord.ProviderId = (int) columns[5];
        claimRecord.ClaimAmount = (decimal) columns[6];
        if (claimRecord.ClaimType == ClaimType.Prescription)
            claimRecord.DrugId = (int) columns[7];
        else
        {
            claimRecord.ProcedureDate = (DateTime) columns[7];
            claimRecord.ProcedureCode = (string) columns[8];
        }

        return claimRecord;
    }

    public string Hash()
    {
        return $"{ClaimNumber},{ClaimLineNumber},{PatientId},{ProviderId},{ProcedureDate},{ProcedureCode}";
    }
}