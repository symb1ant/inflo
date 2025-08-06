using Microsoft.AspNetCore.Mvc;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserLogController(IUserLogService userLogService) : ControllerBase
{
    [HttpGet("list")]
    public IActionResult List()
    {
        var logs = userLogService.GetAllLogs();
        return Ok(logs);
    }

    [HttpGet("details/{id}")]
    public IActionResult Details(long id)
    {
        var log = userLogService.GetAllLogs().FirstOrDefault(x => x.Id == id);
        if (log == null)
        {
            return NotFound();
        }

        return Ok(log);
    }
}
