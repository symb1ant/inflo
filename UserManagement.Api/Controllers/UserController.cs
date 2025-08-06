using Microsoft.AspNetCore.Mvc;
using UserManagement.Contracts.Users;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("list")]
    public IActionResult List()
    {
        var items = userService.GetAll().Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth,
            ChangeLogs = p.ChangeLogs.Select(c => new UserChangeLogViewModel
            {
                Id = c.Id,
                UserId = c.UserId,
                FieldName = c.FieldName,
                OldValue = c.OldValue,
                NewValue = c.NewValue,
                ChangedAt = c.ChangedAt
            }).ToList()
        });

        var result = new UserListViewModel { Items = items.ToList() };

        return Ok(result);
    }

    [HttpGet("list/{isActive}")]
    public IActionResult List(bool isActive)
    {
        var items = userService.FilterByActive(isActive).Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });

        var result = new UserListViewModel { Items = items.ToList() };

        return Ok(result);
    }

    [HttpGet("details/{id}")]
    public IActionResult Details(long id)
    {
        var user = userService.GetAll().FirstOrDefault(x => x.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        var result = new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth,
            ChangeLogs = user.ChangeLogs.Select(c => new UserChangeLogViewModel
            {
                Id = c.Id,
                UserId = c.UserId,
                FieldName = c.FieldName,
                OldValue = c.OldValue,
                NewValue = c.NewValue,
                ChangedAt = c.ChangedAt
            }).ToList()

        };
        return Ok(result);
    }

    [HttpPost("create")]
    public IActionResult Create(UserAddViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new User
        {
            Forename = model.Forename!,
            Surname = model.Surname!,
            Email = model.Email!,
            IsActive = model.IsActive,
            DateOfBirth = model.DateOfBirth
        };

        if (userService.Add(user))
        {
            return CreatedAtAction(nameof(Details), new { id = user.Id }, user);
        }

        return BadRequest();
    }

    [HttpPut("update/{id}")]
    public IActionResult Update(long id, UserAddViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingUser = userService.GetAll().FirstOrDefault(x => x.Id == id);
        if (existingUser == null)
        {
            return NotFound();
        }

        existingUser.Forename = model.Forename!;
        existingUser.Surname = model.Surname!;
        existingUser.Email = model.Email!;
        existingUser.IsActive = model.IsActive;
        existingUser.DateOfBirth = model.DateOfBirth;

        if (userService.Update(existingUser))
        {
            return NoContent();
        }

        return BadRequest();
    }

    [HttpDelete("delete/{id}")]
    public IActionResult Delete(long id)
    {
        var user = userService.GetAll().FirstOrDefault(x => x.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        if (userService.Delete(id))
        {
            return NoContent();
        }

        return BadRequest();
    }
}
