using Microsoft.AspNetCore.Mvc;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserLogController(IUserLogService userLogService) : ControllerBase
{
    [HttpGet("list/{page:int=1}/{pageSize:int=10}")]
    public IActionResult List(int page = 1, int pageSize = 10)
    {
        var skip = (page - 1) * pageSize;
        var logs = userLogService.GetAllLogs().Skip(skip).Take(pageSize).ToList();
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
