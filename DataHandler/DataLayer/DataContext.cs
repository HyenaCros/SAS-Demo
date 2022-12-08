using DataHandler.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataHandler.DataLayer;

public class DataContext : DbContext
{
    public DbSet<ErrorModel> Errors { get; set; }
    public DbSet<MedicalClaimModel> MedicalClaims { get; set; }
    public DbSet<HospitalClaimModel> HospitalClaims { get; set; }
    public DbSet<DentalClaimModel> DentalClaims { get; set; }
    public DbSet<PrescriptionClaimModel> PrescriptionClaims { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}