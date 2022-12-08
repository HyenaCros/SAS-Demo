using AutoMapper;
using DataHandler.DataLayer.Models;
using Shared;
using Validator.Models;

namespace DataHandler.Profiles;

public class DataProfiles : Profile
{
    public DataProfiles()
    {
        CreateMap<ClaimRecord, MedicalClaimModel>();
        CreateMap<ClaimRecord, HospitalClaimModel>();
        CreateMap<ClaimRecord, DentalClaimModel>();
        CreateMap<ClaimRecord, PrescriptionClaimModel>();

        CreateMap<ErrorRecord, ErrorModel>()
            .ReverseMap();
    }
}