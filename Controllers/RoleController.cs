using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

 namespace AuthServer.Controllers;

[Authorize(Roles = "SysAdmin")]
 public class RoleController : Controller {
     private readonly RoleManager<IdentityRole> _roleManager;

     public RoleController(RoleManager<IdentityRole> roleManager) {
         _roleManager = roleManager;
     }

     [Route("/administration/roles")]
     public async Task<IActionResult> Index() {
         var roles = await _roleManager.Roles.ToListAsync();

         return View(roles);
     }

     public async Task<IActionResult> AddRole(string roleName) {
         if (roleName != null) {
             await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
         }

         return RedirectToAction("Index");
     }
 }