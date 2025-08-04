using System;

namespace UserManagement.Models;

public class UserChangeLog
{
    public User? User { get; set; } = default!;

    public long Id { get; set; }
    public long UserId { get; set; }
    public string FieldName { get; set; } = default!;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime ChangedAt { get; set; }
}
