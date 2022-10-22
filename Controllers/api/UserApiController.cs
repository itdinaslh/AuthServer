using AuthServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace AuthServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserApiController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserApiController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }


}
