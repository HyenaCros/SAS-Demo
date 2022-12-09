using System.Text;
using AutoMapper;
using DataHandler.DataLayer;
using DataHandler.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Dtos;
using Shared.Services;

namespace DataHandler.Profiles.Services;

public class UploadsService
{
    private readonly IMapper _mapper;
    private readonly FileStorageService _fileStorageService;
    private readonly ValidationService _validationService;
    private readonly DataContext _dataContext;

    public UploadsService(IMapper mapper, FileStorageService fileStorageService, ValidationService validationService, DataContext dataContext)
    {
        _mapper = mapper;
        _fileStorageService = fileStorageService;
        _validationService = validationService;
        _dataContext = dataContext;
    }
    
    public async Task<List<Guid>> AddFileUploads(AddFileUploadsRequest request)
    {
        var fileUploads = _mapper.Map<List<FileUploadModel>>(request);
        foreach (var fileUpload in fileUploads)
        {
            fileUpload.Status = fileUpload.Errors.Count > 0 ? UploadStatus.Failure : UploadStatus.Processing;
        }
        _dataContext.FileUploads.AddRange(fileUploads);
        await _dataContext.SaveChangesAsync();
        var ids = fileUploads
            .Where(x => x.Status == UploadStatus.Processing || request.IncludeFailures)
            .Select(x => x.Id)
            .ToList();
        return ids;
    }
    
    public async Task UpdateFileUpload(ValidationResult request)
    {
        var fileUpload = await _dataContext.FileUploads.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (fileUpload == null)
            throw new KeyNotFoundException();
        if (fileUpload.Status != UploadStatus.Processing)
            throw new ArgumentException();

        if (request.ClaimRecords.Count > 0)
        {
            fileUpload.Status = UploadStatus.Success;
            var medicalModels = request.ClaimRecords
                .Where(x => x.ClaimType == ClaimType.Medical)
                .Select(x => _mapper.Map<MedicalClaimModel>(x))
                .ToList();
        
            var hospitalModels = request.ClaimRecords
                .Where(x => x.ClaimType == ClaimType.Hospital)
                .Select(x => _mapper.Map<HospitalClaimModel>(x))
                .ToList();
        
            var dentalModels = request.ClaimRecords
                .Where(x => x.ClaimType == ClaimType.Dental)
                .Select(x => _mapper.Map<DentalClaimModel>(x))
                .ToList();
        
            var prescriptionModels = request.ClaimRecords
                .Where(x => x.ClaimType == ClaimType.Prescription)
                .Select(x => _mapper.Map<PrescriptionClaimModel>(x))
                .ToList();

            fileUpload.MedicalClaims = medicalModels;
            fileUpload.HospitalClaims = hospitalModels;
            fileUpload.DentalClaims = dentalModels;
            fileUpload.PrescriptionClaims = prescriptionModels;
        }

        if (request.ErrorRecords.Count > 0)
        {
            fileUpload.Status = request.ClaimRecords.Count > 0 ? UploadStatus.ContainsErrors : UploadStatus.Failure;
            var errorModels = _mapper.Map<List<ErrorModel>>(request.ErrorRecords);
            fileUpload.Errors = errorModels;
        }

        if (request.ErrorRecords.Count == 0 && request.ClaimRecords.Count == 0)
        {
            fileUpload.Status = UploadStatus.Failure;
        }

        await _dataContext.SaveChangesAsync();
    }

    public async Task<FileUploadRecord> GetFileUpload(Guid id)
    {
        var record = await _dataContext.FileUploads
            .Include(x => x.Errors)
            .Include(x => x.MedicalClaims)
            .Include(x => x.HospitalClaims)
            .Include(x => x.DentalClaims)
            .Include(x => x.PrescriptionClaims)
            .FirstOrDefaultAsync(x => x.Id == id);
        return record == null ? null : _mapper.Map<FileUploadRecord>(record);
    }

    public async Task<List<FileUploadRecord>> GetFileUploads()
    {
        var fileUploads = await _dataContext.FileUploads
            .OrderByDescending(x => x.DateUploaded)
            .ToListAsync();

        return _mapper.Map<List<FileUploadRecord>>(fileUploads);
    }

    public async Task<List<ErrorLogRecord>> GetErrorLog()
    {
        var errors = await _dataContext.Errors
            .Include(x => x.FileUpload)
            .OrderByDescending(x => x.FileUpload.DateUploaded)
            .ThenBy(x => x.LineNumber)
            .ToListAsync();
        return _mapper.Map<List<ErrorLogRecord>>(errors);
    }

    public async Task<FileUploadRecord> ValidateUserUpload(IFormFile file, ClaimType type)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        var contents = Encoding.UTF8.GetString(stream.ToArray());
        await _fileStorageService.SaveFile(file.FileName, contents);
        var fileUpload = new FileUploadModel()
        {
            FileName = file.FileName,
            DateUploaded = DateTime.Now,
            Type = type,
            Source = FileSource.User
        };
        _dataContext.FileUploads.Add(fileUpload);
        await _dataContext.SaveChangesAsync();
        await _validationService.ValidateFiles(new List<Guid>() { fileUpload.Id });
        return _mapper.Map<FileUploadRecord>(fileUpload);
    }
}