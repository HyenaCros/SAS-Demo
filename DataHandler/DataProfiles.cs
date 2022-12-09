using AutoMapper;
using DataHandler.DataLayer.Models;
using Shared;
using Shared.Dtos;
using Validator.Models;

namespace DataHandler.Profiles;

public class DataProfiles : Profile
{
    public DataProfiles()
    {
        CreateMap<BaseFileUpload, FileUploadModel>();
        CreateMap<FileUploadModel, FileUploadRecord>();

        CreateMap<ClaimRecord, MedicalClaimModel>();
        CreateMap<MedicalClaimModel, FileClaimRecord>();
        CreateMap<ClaimRecord, HospitalClaimModel>();
        CreateMap<HospitalClaimModel, FileClaimRecord>();
        CreateMap<ClaimRecord, DentalClaimModel>();
        CreateMap<DentalClaimModel, FileClaimRecord>();
        CreateMap<ClaimRecord, PrescriptionClaimModel>();
        CreateMap<PrescriptionClaimModel, FileClaimRecord>();

        CreateMap<ErrorRecord, ErrorModel>()
            .ReverseMap();

        CreateMap<ErrorModel, ErrorLogRecord>()
            .ForMember(x => x.FileUploadId,
                opt => opt.MapFrom(src => src.FileUpload.Id))
            .ForMember(x => x.FileName,
                opt => opt.MapFrom(src => src.FileUpload.FileName))
            .ForMember(x => x.DateCreated,
                opt => opt.MapFrom(src => src.FileUpload.DateUploaded));
    }
}