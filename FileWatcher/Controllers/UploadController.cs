using System.Text;
using FileWatcher.Services;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace FileWatcher.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly ValidationService _validationService;
    private readonly FileStorageService _fileStorageService;
    private readonly DataHandlerService _dataHandlerService;

    public UploadController(ValidationService validationService, FileStorageService fileStorageService, DataHandlerService dataHandlerService)
    {
        _validationService = validationService;
        _fileStorageService = fileStorageService;
        _dataHandlerService = dataHandlerService;
    }
    [HttpPost]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest request)
    {
        if (!Enum.TryParse<ClaimType>(request.Type, out var claimType))
            return BadRequest();
        await ValidateFile(request.File, claimType);
        return Ok();
    }

    [HttpGet("Errors")]
    public async Task<IActionResult> GetErrors()
    {
        var records = await _dataHandlerService.GetErrorRecords();
        return Ok(records);
    }

    private async Task ValidateFile(IFormFile file, ClaimType type)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        var contents = Encoding.UTF8.GetString(stream.ToArray());
        await _fileStorageService.SaveFile(file.FileName, contents);
        await _validationService.ValidateFiles(new List<FileData>()
        {
            new FileData()
            {
                FileName = file.FileName,
                FileType = type,
                Contents = contents
            }
        });
    }
}

public class UploadFileRequest
{
    public string Type { get; set; }
    public IFormFile File { get; set; }
}