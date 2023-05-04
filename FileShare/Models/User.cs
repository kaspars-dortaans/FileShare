using FileShare.Enums;

namespace FileShare.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Username { get; set; } = default!;
    public Role Role { get; set; }
    public string PasswordHash { get; set; } = default!;
}