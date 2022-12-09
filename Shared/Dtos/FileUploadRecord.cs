using Validator.Models;

namespace Shared.Dtos;

public class FileUploadRecord
{
    public Guid Id { get; set; }
    public DateTime DateUploaded { get; set; }
    public UploadStatus Status { get; set; }
    public string FileName { get; set; }
    public ClaimType Type { get; set; }
    public FileSource Source { get; set; }

    public List<ErrorRecord> Errors { get; set; }
    public List<FileClaimRecord> MedicalClaims { get; set; }
    public List<FileClaimRecord> HospitalClaims { get; set; }
    public List<FileClaimRecord> DentalClaims { get; set; }
    public List<FileClaimRecord> PrescriptionClaims { get; set; }
}