using System.ComponentModel.DataAnnotations;

namespace UserManagement.Contracts.Users;

public class UserAddViewModel
{
    [Required(ErrorMessage = "Forename is required")]
    public string? Forename { get; set; }
    [Required(ErrorMessage = "Surname is required")]
    public string? Surname { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
    [Required(ErrorMessage = "Date of Birth is required")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date of Birth")]
    [Range(typeof(DateTime), "1/1/1900", "01/01/2025", ErrorMessage = "Date of Birth must be between 1/1/1900 and 01/01/2025")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; } = DateTime.Now;
}
