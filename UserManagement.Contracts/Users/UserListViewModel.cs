namespace UserManagement.Contracts.Users;

public class UserListViewModel
{
    public List<UserListItemViewModel> Items { get; set; } = new();
}

public class UserListItemViewModel
{
    public long Id { get; set; }
    public string? Forename { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateOfBirth { get; set; }
    public List<UserChangeLogViewModel> ChangeLogs { get; set; } = new();
}

public class UserChangeLogViewModel
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string FieldName { get; set; } = default!;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime ChangedAt { get; set; }
}
