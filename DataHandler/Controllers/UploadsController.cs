using System.Text;
using AutoMapper;
using DataHandler.DataLayer;
using DataHandler.DataLayer.Models;
using DataHandler.Profiles.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Dtos;

namespace DataHandler.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadsController : ControllerBase
{
    private readonly UploadsService _uploadsService;

    public UploadsController(UploadsService uploadsService)
    {
        _uploadsService = uploadsService;
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest request)
    {
        if (!Enum.TryParse<ClaimType>(request.Type, out var claimType))
            return BadRequest();
        var fileUpload = await _uploadsService.ValidateUserUpload(request.File, claimType);
        return Ok(fileUpload);
    }
    
    [HttpPost("Bulk")]
    public async Task<IActionResult> AddFileUpload([FromBody] AddFileUploadsRequest request)
    {
        var ids = await _uploadsService.AddFileUploads(request);
        return Ok(ids);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateFileUpload([FromBody] ValidationResult request)
    {
        try
        {
            await _uploadsService.UpdateFileUpload(request);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(request.Id);
        }
        catch (ArgumentException)
        {
            return BadRequest("File already finished processing");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetFileUpload([FromRoute] Guid id)
    {
        var fileUpload = await _uploadsService.GetFileUpload(id);

        return fileUpload == null 
            ? NotFound(id) 
            : Ok(fileUpload);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetFileUploads()
    {
        var fileUploads = await _uploadsService.GetFileUploads();
        return Ok(fileUploads);
    }

    [HttpGet("Errors")]
    public async Task<IActionResult> GetErrorLog()
    {
        var errors = await _uploadsService.GetErrorLog();
        return Ok(errors);
    }
    
}

public class UploadFileRequest
{
    public string Type { get; set; }
    public IFormFile File { get; set; }
}