using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public ViewResult List()
    {
        var items = _userService.GetAll().Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });

        var model = new UserListViewModel { Items = items.ToList() };

        return View(model);
    }

    [HttpGet("list/{isActive}")]
    public ViewResult List(bool isActive)
    {
        var items = _userService.FilterByActive(isActive).Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });

        var model = new UserListViewModel { Items = items.ToList() };

        return View(model);
    }

    [HttpGet("details/{id}")]
    public ViewResult Details(long id)
    {
        var user = _userService.GetAll().FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return View("NotFound");
        }

        var model = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

        return View(model);
    }

    [HttpGet("adduser")]
    public ViewResult AddUser()
    {
        return View(new UserAddViewModel());
    }


    [HttpPost("adduser")]
    public IActionResult AddUser(UserAddViewModel model)
    {
        TempData["UserAdded"] = false;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new User
        {
            Forename = model.Forename!,
            Surname = model.Surname!,
            Email = model.Email!,
            IsActive = model.IsActive,
            DateOfBirth = model.DateOfBirth
        };


        if (!_userService.Add(user))
        {
            ModelState.AddModelError("", "An error occurred while adding the user.");
            return View(model);
        }

        TempData["UserAdded"] = true;
        return RedirectToAction("List");
    }

    [HttpGet("edituser/{id}")]
    public ViewResult EditUser(long id)
    {
        var user = _userService.GetAll().FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return View("NotFound");
        }

        var model = new UserEditViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

        return View(model);
    }

    [HttpPost("edituser/{id}")]
    public IActionResult EditUser(long id, UserEditViewModel model)

    {
        TempData["UserUpdated"] = false;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new User
        {
            Id = id,
            Forename = model.Forename!,
            Surname = model.Surname!,
            Email = model.Email!,
            IsActive = model.IsActive,
            DateOfBirth = model.DateOfBirth
        };

        if (!_userService.Update(user))
        {
            ModelState.AddModelError("", "An error occurred while updating the user.");
            return View(model);
        }

        TempData["UserUpdated"] = true;
        return RedirectToAction("List");
    }
}
