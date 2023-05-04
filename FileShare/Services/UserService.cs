namespace FileShare.Services;

using BCrypt.Net;
using FileShare.Authorization;
using FileShare.Helpers;
using FileShare.Models;
using FileShare.ViewModels.Users;

public class UserService : BaseService<User>, IUserService
{
    private DataContext _context;
    private IJwtUtils _jwtUtils;

    public UserService(
        DataContext dataContext,
        DataContext context,
        IJwtUtils jwtUtils) : base(dataContext)
    {
        _context = context;
        _jwtUtils = jwtUtils;
    }


    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        var user = _context.Users.SingleOrDefault(x => x.Username == model.Username);

        // validate
        if (user == null || !BCrypt.Verify(model.Password, user.PasswordHash))
            throw new AppException("Username or password is incorrect");

        // authentication successful so generate jwt token
        var jwtToken = _jwtUtils.GenerateJwtToken(user);

        return new AuthenticateResponse(user, jwtToken);
    }
}

public interface IUserService : IBaseService<User>
{
    AuthenticateResponse Authenticate(AuthenticateRequest model);
}