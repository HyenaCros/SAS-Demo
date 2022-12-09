using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared;

namespace DataHandler.DataLayer.Models;

public class FileUploadModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public DateTime DateUploaded { get; set; }
    public UploadStatus Status { get; set; }
    public string FileName { get; set; }
    public ClaimType Type { get; set; }
    public FileSource Source { get; set; }

    public List<ErrorModel> Errors { get; set; }
    public List<MedicalClaimModel> MedicalClaims { get; set; }
    public List<HospitalClaimModel> HospitalClaims { get; set; }
    public List<DentalClaimModel> DentalClaims { get; set; }
    public List<PrescriptionClaimModel> PrescriptionClaims { get; set; }
}