using AutoMapper;
using DataHandler.DataLayer;
using DataHandler.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DataHandler.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ErrorsController : ControllerBase
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public ErrorsController(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddErrorRecords([FromBody] AddErrorRecordsRequest request)
    {
        var errorModels = _mapper.Map<List<ErrorModel>>(request.ErrorRecords);
        _dataContext.Errors.AddRange(errorModels);
        await _dataContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetErrorRecords()
    {
        var errors = await _dataContext.Errors.OrderByDescending(x => x.DateCreated).ToListAsync();
        return Ok(_mapper.Map<List<ErrorRecord>>(errors));
    }
}