using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserLogService : IUserLogService
{

    private readonly IDataContext _dataAccess;

    public UserLogService(IDataContext dataAccess) => _dataAccess = dataAccess;

    public IEnumerable<UserChangeLog> GetAllLogs() => _dataAccess.GetAll<UserChangeLog>();

    public IEnumerable<UserChangeLog> GetLogsByUserId(long userId)
    {
        return _dataAccess.GetAll<UserChangeLog>().Where(log => log.UserId == userId);
    }
}
