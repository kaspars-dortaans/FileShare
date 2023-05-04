using FileShare.Enums;

namespace FileShare.ViewModels.Users
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Username { get; set; } = default!;
        public Role Role { get; set; }
    }
}
