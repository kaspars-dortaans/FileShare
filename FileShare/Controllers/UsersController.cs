namespace FileShare.Controllers;

using AutoMapper;
using BCrypt.Net;
using FileShare.Authorization;
using FileShare.Enums;
using FileShare.Helpers;
using FileShare.Models;
using FileShare.Services;
using FileShare.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public IActionResult Register(RegisterUserViewModel model)
    {
        if (_userService.Exists(user => user.Username == model.Username))
            throw new AppException("User with given username already exists");

        var user = _mapper.Map<User>(model);
        user.PasswordHash = BCrypt.HashPassword(model.Password);
        _userService.Add(user);
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Authenticate(AuthenticateRequest model)
    {
        var response = _userService.Authenticate(model);
        return Ok(response);
    }

    [Authorize(Role.Admin)]
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(_mapper.Map<IEnumerable<UserViewModel>>(users));
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        // only admins can access other user records
        var currentUser = (User?)HttpContext.Items["User"];
        if (currentUser == null)
            return NotFound();

        if (id != currentUser?.Id && currentUser!.Role != Role.Admin)
            return Unauthorized(new { message = "Unauthorized" });

        var user = _userService.FindByIdOrDefault(id);
        if (user == null)
            throw new KeyNotFoundException("User not found");\

        return Ok(_mapper.Map<UserViewModel>(user));
    }
}