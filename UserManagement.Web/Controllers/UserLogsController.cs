using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.UserLogs;
using static System.Math;

namespace UserManagement.WebMS.Controllers;

public class UserLogsController : Controller
{
    private readonly IUserLogService _userLogService;

    public UserLogsController(IUserLogService userLogService)
    {
        _userLogService = userLogService;
    }

    [HttpGet("list/{page?}")]
    public ViewResult List(int page = 1)
    {
        const int pageSize = 10;
        var logs = _userLogService.GetAllLogs()
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var viewModel = new UserLogListViewModel
        {
            Items = logs.Select(log => new UserLogListViewModel.UserLogListItemViewModel
            {
                Id = log.Id,
                UserId = log.UserId,
                FieldName = log.FieldName,
                OldValue = log.OldValue,
                NewValue = log.NewValue,
                ChangedAt = log.ChangedAt
            }).ToList()
        };

        viewModel.PageCount = (int)Ceiling((double)_userLogService.GetAllLogs().Count() / pageSize);
        viewModel.CurrentPage = page;

        return View(viewModel);
    }

    [HttpGet("details/{id}")]
    public ViewResult Details(long id)
    {
        var log = _userLogService.GetLogsByUserId(id).FirstOrDefault();
        if (log == null)
        {
            return View("NotFound");
        }

        var viewModel = new UserLogListViewModel.UserLogListItemViewModel
        {
            Id = log.Id,
            UserId = log.UserId,
            FieldName = log.FieldName,
            OldValue = log.OldValue,
            NewValue = log.NewValue,
            ChangedAt = log.ChangedAt
        };

        return View(viewModel);
    }
}
