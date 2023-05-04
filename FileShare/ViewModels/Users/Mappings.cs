using FileShare.Models;

namespace FileShare.ViewModels.Users;

public class UsersProfile : AutoMapper.Profile
{
    public UsersProfile()
    {
        CreateMap<User, UserViewModel>();
        CreateMap<RegisterUserViewModel, User>();
    }
}
