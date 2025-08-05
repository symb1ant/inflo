using UserManagement.Services.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Models;
using UserManagement.Web.Models.UserLogs;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserLogControllerTests
{
    [Fact]
    public void List_WhenServiceReturnsUserLogs_ModelMustContainUserLogs()
    {
        // Arrange
        var userLogs = SetupUserLogs();
        var controller = CreateController();

        // Act
        var result = controller.List(1);

        // Assert
        result.Should().NotBeNull();

        result.Model
            .Should().BeOfType<UserLogListViewModel>()
            .Which.Items.Should().BeEquivalentTo(userLogs.Take(10),  options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void Details_WhenServiceReturnsUserLog_ModelMustContainUserLog()
    {
        // Arrange
        var userLogs = SetupUserLogs();
        var controller = CreateController();
        var filteredLogs = userLogs.Where(x => x.UserId == userLogs[0].UserId).ToList();

        // Act
        var result = controller.Details(userLogs[0].UserId);

        // Assert
        result.Model
            .Should().BeOfType<UserLogListViewModel.UserLogListItemViewModel>()
            .Which.Should().BeEquivalentTo(filteredLogs.FirstOrDefault(), options => options.ExcludingMissingMembers());
    }

    private List<UserChangeLog> SetupUserLogs()
    {
        var userLogs = new List<UserChangeLog>();

        for (long i = 1; i < 11; i++)
        {
            userLogs.Add(new UserChangeLog
            {
                Id = i,
                FieldName = "Email",
                OldValue = $"Email{i}@test.com",
                NewValue = $"New{i}@test.com",
                ChangedAt = DateTime.UtcNow.AddDays(+i),
                User = new User
                {
                    Id = i,
                    Forename = $"User{i}",
                    Surname = $"Surname{i}",
                    Email = $"Email{i}@test.com",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    IsActive = true
                }
            });
        }

        _userLogService.Setup(service => service.GetAllLogs())
            .Returns(userLogs);

        _userLogService.Setup(service => service.GetLogsByUserId(It.IsAny<long>()))
            .Returns((long id) => userLogs.Where(log => log.UserId == id));

        return userLogs;
    }

    private readonly Mock<IUserLogService> _userLogService = new();
    private UserLogsController CreateController() => new(_userLogService.Object);
}
