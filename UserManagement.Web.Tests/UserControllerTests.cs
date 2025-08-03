using System;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.List();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void List_WhenServiceReturnsActiveUsers_ModelMustContainActiveUsers()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers(isActive: true);

        // Act
        var result = controller.List(true);

        // Assert
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void List_WhenServiceReturnsInactiveUsers_ModelMustContainInactiveUsers()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers(isActive: false);

        // Act
        var result = controller.List(false);

        // Assert
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void Details_WhenServiceReturnsUser_ModelMustContainUser()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        // Act
        var result = controller.Details(users[0].Id);

        // Assert
        var model = result.Model.Should().BeOfType<UserListItemViewModel>().Subject;
        model.Id.Should().Be(users[0].Id);
        model.Forename.Should().Be(users[0].Forename);
        model.Surname.Should().Be(users[0].Surname);
        model.Email.Should().Be(users[0].Email);
        model.DateOfBirth.Should().Be(users[0].DateOfBirth);
        model.IsActive.Should().Be(users[0].IsActive);
    }

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", DateTime dateOfBirth = default, bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive,
                DateOfBirth = dateOfBirth == default ? new DateTime ( 2004, 11, 1 ) : dateOfBirth
            }
        };

        _userService
            .Setup(s => s.GetAll())
            .Returns(users);

        _userService
            .Setup(s => s.FilterByActive(isActive))
            .Returns(users);

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private UsersController CreateController() => new(_userService.Object);
}
