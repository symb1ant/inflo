namespace UserManagement.Contracts.UserLogs;

public class UserLogListViewModel
{
    public List<UserLogListItemViewModel> Items { get; set; } = new();

    public class UserLogListItemViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? FieldName { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime ChangedAt { get; set; }
    }

    public int PageCount { get; set; } = 0;
    public int CurrentPage { get; set; } = 1;
}
