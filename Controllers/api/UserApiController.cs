using AuthServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using AuthServer.Helpers;
using AuthServer.Models;

namespace AuthServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserApiController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;   

    public UserApiController(
         UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        SignInManager<ApplicationUser> signInManager
    ) {
         _userManager = userManager;
        _userStore = userStore;        
        _signInManager = signInManager;
        _emailStore = GetEmailStore();
    }

    // [HttpPost("/api/users/list")]
    // public async Task<IActionResult> ListAllUsers()
    // {
    //     var draw = Request.Form["draw"].FirstOrDefault();
    //     var start = Request.Form["start"].FirstOrDefault();
    //     var length = Request.Form["length"].FirstOrDefault();
    //     var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
    //     var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
    //     var searchValue = Request.Form["search[value]"].FirstOrDefault();
    //     int pageSize = length != null ? Convert.ToInt32(length) : 0;
    //     int skip = start != null ? Convert.ToInt32(start) : 0;
    //     int recordsTotal = 0;

    //     var init = _userManager.Users;

    // }

    private ApplicationUser CreateUser() {
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

    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)_userStore;
    }

    [HttpPost("/api/user/store")]
    public async Task<IActionResult> StoreNewUser() {
        var namaUser = Request.Form["UserName"].FirstOrDefault();
        var myEmail = Request.Form["Email"].FirstOrDefault();
        var isExist = await _userManager.Users.Where(x => x.UserName == namaUser || x.Email == myEmail).FirstOrDefaultAsync();

        if (isExist is not null) {
            return new JsonResult(Result.Exists());
        }

        var user = CreateUser();
        user.FullName = Request.Form["FullName"].FirstOrDefault();

        await _userStore.SetUserNameAsync(user, Request.Form["UserName"].FirstOrDefault(), CancellationToken.None);
        await _emailStore.SetEmailAsync(user, Request.Form["Email"].FirstOrDefault(), CancellationToken.None);
        var result = await _userManager.CreateAsync(user, Request.Form["Password"].FirstOrDefault());

        if (result.Succeeded) {
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var role = await _userManager.AddToRoleAsync(user, Request.Form["Roles"].FirstOrDefault());
            var pjlp = await _userManager.AddToRoleAsync(user, "PjlpUser");

            return new JsonResult(Result.Success());
        }

        return new JsonResult(Result.Failed());
    }

    [HttpPost("/api/user/driver")]
    public async Task<IActionResult> StoreDriver(UserModel model) {
        var isExist = await _userManager.Users.Where(u => u.UserName == model.UserName).FirstOrDefaultAsync();

        if (isExist is not null) {
            return new JsonResult(Result.Exists());
        }

        var user = CreateUser();
        user.FullName = model.FullName;

        await _userStore.SetUserNameAsync(user, model.UserName, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded) {
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var role = await _userManager.AddToRoleAsync(user, model.Roles);
            var pjlp = await _userManager.AddToRoleAsync(user, "PjlpUser");

            return new JsonResult(Result.Success());
        }

        return StatusCode(500, "Something wrong!");
    }
}
