using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AuthServer.Data;

namespace AuthServer.Controllers;

[Authorize(Roles = "SysAdmin")]
public class UserController : Controller {
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(UserManager<ApplicationUser> userManager) {
        _userManager = userManager;
    }

    [Route("/administration/users")]
    public async Task<IActionResult> Index() {
        var allUser = await _userManager.Users.ToListAsync();

        return View(allUser);
    }
    
}

