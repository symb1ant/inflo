using Microsoft.AspNetCore.Mvc;
using UserManagement.Contracts.UserLogs;
using UserManagement.Services.Domain.Interfaces;
using static System.Math;

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

        if (!logs.Any())
        {
            return Ok(new UserLogListViewModel());
        }

        var result = new UserLogListViewModel
        {
            Items = logs.Select(log => new UserLogListViewModel.UserLogListItemViewModel
            {
                Id = log.Id,
                UserId = log.UserId,
                FieldName = log.FieldName,
                OldValue = log.OldValue,
                NewValue = log.NewValue,
                ChangedAt = log.ChangedAt
            }).ToList(),
            PageCount = (int)Ceiling((double)userLogService.GetAllLogs().Count() / pageSize),
            CurrentPage = page
        };

        return Ok(result);
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
