using System.Collections.Generic;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserLogService
{
    public IEnumerable<UserChangeLog> GetAllLogs();
    public IEnumerable<UserChangeLog> GetLogsByUserId(long userId);
}
