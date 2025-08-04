using System;
using System.Linq;
using FluentAssertions;
using UserManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Data.Tests;

public class DataContextTests
{
    [Fact]
    public void GetAll_WhenNewEntityAdded_MustIncludeNewEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();

        var entity = new User
        {
            Forename = "Brand New",
            Surname = "User",
            Email = "brandnewuser@example.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = true
        };
        context.Create(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result
            .Should().Contain(s => s.Email == entity.Email)
            .Which.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public void GetAll_WhenExistingEntityUpdated_MustIncludeUpdatedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();

        var entity = context.GetAll<User>().First();
        entity.Forename = "Updated";
        context.Update(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();
        // Assert: Verifies that the action of the method under test behaves as expected.

        result.First(s => s.Email == entity.Email).Forename.Should().Be("Updated");
    }

    [Fact]
    public void GetAll_WhenDeleted_MustNotIncludeDeletedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var entity = context.GetAll<User>().First();
        context.Delete(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotContain(s => s.Email == entity.Email);
    }

    [Fact]
    public void UpdateUser_ShouldCreateChangeLog_WhenPropertyIsModified()
    {
        // Arrange
        var context = CreateContext();
        var user = new User { Forename = "John", Surname = "Doe", Email = "john@example.com", IsActive = true };
        context.Create(user);

        // Act
        user.Forename = "Jane"; // Make sure this is the last modification
        context.Update(user);

        // Assert
        var result = context.GetAll<User>().Include(u => u.ChangeLogs).ToList();
        var forenameChangeLog = result.First(s => s.Email == user.Email)
            .ChangeLogs.FirstOrDefault(cl => cl.FieldName == "Forename");
        forenameChangeLog.Should().NotBeNull();
        forenameChangeLog!.OldValue.Should().Be("John");
        forenameChangeLog!.NewValue.Should().Be("Jane");
    }

    private DataContext CreateContext() => new();
}
