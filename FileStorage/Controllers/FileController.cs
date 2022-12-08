using System.Web;
using Microsoft.AspNetCore.Mvc;
namespace FileStorage.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SaveFile([FromForm] SaveFileRequest request)
    {
        var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "Files"));
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        await using var fileStream = new FileStream(Path.Combine(path, request.File.FileName), FileMode.Create);
        await request.File.CopyToAsync(fileStream);
        return Ok();
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> GetFile([FromRoute] string fileName)
    {
        var path = Path.Combine(Environment.CurrentDirectory, "Files", HttpUtility.UrlDecode(fileName));
        var file = await System.IO.File.ReadAllTextAsync(path);
        return Ok(file);
    }
}

public class SaveFileRequest
{
    public IFormFile File { get; set; }
}