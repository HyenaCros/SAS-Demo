using AutoMapper;
using DataHandler.DataLayer;
using DataHandler.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Validator.Models;

namespace DataHandler.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public ClaimsController(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddClaimRecords([FromBody] AddClaimRecordsRequest request)
    {
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
        
        _dataContext.MedicalClaims.AddRange(medicalModels);
        _dataContext.HospitalClaims.AddRange(hospitalModels);
        _dataContext.DentalClaims.AddRange(dentalModels);
        _dataContext.PrescriptionClaims.AddRange(prescriptionModels);
        
        await _dataContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRecords()
    {
        var medicalClaims = await _dataContext.MedicalClaims.ToListAsync();
        var hospitalClaims = await _dataContext.HospitalClaims.ToListAsync();
        var dentalClaims = await _dataContext.DentalClaims.ToListAsync();
        var prescriptionClaims = await _dataContext.PrescriptionClaims.ToListAsync();

        return Ok(new
        {
            MedicalClaims = medicalClaims,
            HopsitalClaims = hospitalClaims,
            DentalClaims = dentalClaims,
            PrescriptionClaims = prescriptionClaims
        });
    }
}