using Microsoft.AspNetCore.Mvc;
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
        var users = userService.GetAll();
        return Ok(users);
    }

    [HttpGet("details/{id}")]
    public IActionResult Details(long id)
    {
        var user = userService.GetAll().FirstOrDefault(x => x.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost("create")]
    public IActionResult Create(User model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (userService.Add(model))
        {
            return CreatedAtAction(nameof(Details), new { id = model.Id }, model);
        }

        return BadRequest();
    }

    [HttpPut("update/{id}")]
    public IActionResult Update(long id, User model)
    {
        if (id != model.Id)
        {
            return BadRequest("User ID mismatch.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingUser = userService.GetAll().FirstOrDefault(x => x.Id == id);
        if (existingUser == null)
        {
            return NotFound();
        }

        if (userService.Update(model))
        {
            return NoContent();
        }

        return BadRequest();
    }
}
