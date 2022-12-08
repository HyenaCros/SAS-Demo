using Microsoft.AspNetCore.Mvc;
using Shared;
using Validator.Services;

namespace Validator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ValidationController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Validate([FromBody] ValidateFilesRequest request)
    {
        foreach (var file in request.Files)
        {
            BackgroundQueueService.Queue.Enqueue(file);
        }
        return Ok();
    }
}