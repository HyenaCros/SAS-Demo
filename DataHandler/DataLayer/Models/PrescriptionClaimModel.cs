using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared;

namespace DataHandler.DataLayer.Models;

public class PrescriptionClaimModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public ClaimType ClaimType { get; set; }
    public int ClaimNumber { get; set; }
    public DateTime ClaimDate { get; set; }
    public int ClaimLineNumber { get; set; }
    public int PatientId { get; set; }
    public int ProviderId { get; set; }
    public decimal ClaimAmount { get; set; }
    public int DrugId { get; set; }
}