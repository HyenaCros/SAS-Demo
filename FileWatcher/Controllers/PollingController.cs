using FileWatcher.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileWatcher.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PollingController : ControllerBase
{
    [HttpPost("Start")]
    public async Task<IActionResult> Start()
    {
        BackgroundPollingService.Start();
        return Ok();
    }

    [HttpPost("Stop")]
    public async Task<IActionResult> Stop()
    {
        BackgroundPollingService.Stop();
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Status()
    {
        return Ok(new
        {
            Status = BackgroundPollingService.Enabled
        });
    }
}