using System.ComponentModel.DataAnnotations;

namespace FileShare.ViewModels.Users;

public class RegisterUserViewModel
{
    [Required]
    public string FirstName { get; set; } = default!;
    [Required]
    public string LastName { get; set; } = default!;
    [Required]
    public string Username { get; set; } = default!;
    [Required]
    public string Password { get; set; } = default!;
    [Required]
    public string ConfirmPassword { get; set; } = default!;
}
