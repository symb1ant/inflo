using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserLogServiceTests
{
    [Fact]
    public void GetAllLogs_ReturnsAllLogs()
    {
        // Arrange
        var service = CreateService();
        var logs = new List<UserChangeLog> { CreateNewLog(), CreateNewLog() };
        _dataContext.Setup(x => x.GetAll<UserChangeLog>()).Returns(logs.AsQueryable());

        // Act
        var result = service.GetAllLogs();

        // Assert
        result.Count().Should().Be(2);
    }

    [Fact]
    public void GetLogsByUserId_ReturnsLogsForGivenUserId()
    {
        // Arrange
        var service = CreateService();
        var userId = 1;
        var logs = new List<UserChangeLog> { CreateNewLog(), CreateNewLog() };
        logs[0].UserId = userId; // Set UserId for the first log
        _dataContext.Setup(x => x.GetAll<UserChangeLog>()).Returns(logs.AsQueryable());

        // Act
        var result = service.GetLogsByUserId(userId).ToList();

        // Assert
        result.Count.Should().Be(1);
        result.First().UserId.Should().Be(userId);
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private UserLogService CreateService() => new(_dataContext.Object);

    private static UserChangeLog CreateNewLog()
    {
        return new UserChangeLog()
        {
            FieldName = "Email",
            OldValue = "test@test.com",
            NewValue = "new@test.com",
            ChangedAt = DateTime.UtcNow,
            User = new User
            {
                Forename = "Test",
                Surname = "User",
                Email = "test@test.com",
                IsActive = true,
                DateOfBirth = new DateTime(1990, 1, 1)
            },
        };
    }
}
