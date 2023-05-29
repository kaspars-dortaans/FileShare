using FileShare.Authorization;
using FileShare.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileShare.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class BaseController : ControllerBase
{
    protected User user => (User)HttpContext.Items["User"]!;

    public BaseController()
    { }
}
