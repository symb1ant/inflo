using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void FilterByActive_WhenContextReturnsEntities_MustReturnActiveEntities()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers(isActive: true);

        // Act
        var result = service.FilterByActive(true);

        // Assert
        result.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void FilterByActive_WhenContextReturnsInactiveEntities_MustReturnInactivEntities()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers(isActive: false);

        // Act
        var result = service.FilterByActive(false);

        // Assert
        result.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void CreateUser_WhenContextAddsEntity_MustReturnNewEntity()
    {
        // Arrange
        var service = CreateService();

        var user = CreateNewUser();
        _dataContext
            .Setup(s => s.Create(It.IsAny<User>()))
            .Callback<User>(u => u.Id = 1)
            .Verifiable();

        // Act
        var result = service.Add(user);

        // Assert
        result.Should().BeTrue();
        _dataContext.Verify(s => s.Create(It.Is<User>(u => u.Forename == user.Forename &&
                                                           u.Surname == user.Surname &&
                                                           u.Email == user.Email &&
                                                           u.DateOfBirth == user.DateOfBirth &&
                                                           u.IsActive == user.IsActive)), Times.Once);
    }

    [Fact]
    public void UpdateUser_WhenContextUpdatesEntity_MustReturnUpdatedEntity()
    {

        // Arrange
        var service = CreateService();
        var user = CreateNewUser();
        user.Id = 1;

        _dataContext
            .Setup(s => s.Update(It.IsAny<User>()))
            .Callback<User>(u => u.Forename = "Updated")
            .Verifiable();

        // Act
        var result = service.Update(user);

        // Assert
        result.Should().BeTrue();
        _dataContext.Verify(s => s.Update(It.Is<User>(u => u.Id == user.Id &&
                                                           u.Forename == "Updated" &&
                                                           u.Surname == user.Surname &&
                                                           u.Email == user.Email &&
                                                           u.DateOfBirth == user.DateOfBirth &&
                                                           u.IsActive == user.IsActive)), Times.Once);

    }

    [Fact]
    public void DeleteUser_WhenContextDeletesEntity_MustReturnTrue()
    {
        // Arrange
        var service = CreateService();
        var user = CreateNewUser();
        user.Id = 1;

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(new[] { user }.AsQueryable());

        _dataContext
            .Setup(s => s.Delete(It.IsAny<User>()))
            .Verifiable();

        // Act
        var result = service.Delete(user.Id);

        // Assert
        result.Should().BeTrue();
        _dataContext.Verify(s => s.Delete(It.Is<User>(u => u.Id == user.Id)), Times.Once);
    }

    private IQueryable<User> SetupUsers(string forename = "Johnny", string surname = "User",
        string email = "juser@example.com", DateTime dateOfBirth = default, bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                DateOfBirth = dateOfBirth == default ? new DateTime(2004, 11, 1) : dateOfBirth,
                IsActive = isActive
            }
        }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        return users;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private UserService CreateService() => new(_dataContext.Object);

    private static User CreateNewUser()
    {
        return new User
        {
            Forename = "New",
            Surname = "User",
            Email = "new@user.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            IsActive = true
        };
    }
}
