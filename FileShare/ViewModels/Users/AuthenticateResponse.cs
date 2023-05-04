namespace FileShare.ViewModels.Users;

using FileShare.Models;

public class AuthenticateResponse
{
    public UserViewModel UserData { get; set; }
    public string Token { get; set; }

    public AuthenticateResponse(User user, string token)
    {
        UserData = new UserViewModel()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Role = user.Role
        };
        Token = token;
    }
}