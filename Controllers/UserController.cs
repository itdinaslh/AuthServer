using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AuthServer.Data;
using AuthServer.Models;
using AuthServer.Helpers;

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

    [HttpGet("/administration/users/create")]
    public IActionResult Create() {
        return PartialView("~/Views/User/Add.cshtml", new UserAdmin());
    }

    [HttpPost("/administration/users/store")]
    public async Task<IActionResult> Store(UserAdmin model) {
        if (ModelState.IsValid) {
            var user = CreateUser();

            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded) {
                return Json(Result.Success());
            }
        }

        return PartialView("~/Views/User/Add.cshtml", model);
    }

     private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }
    
}

