using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Core;
using OpenIddict.Abstractions;
using AuthServer.Helpers;
using AuthServer.Data;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthServer.Controllers;

[Authorize(Roles = "SysAdmin")]
public class AppController : Controller
{
    private readonly IServiceProvider _serviceProvider;

    private ApplicationDbContext context;

    public AppController(IServiceProvider serviceProvider, ApplicationDbContext ctx)
    {
        _serviceProvider = serviceProvider;
        context = ctx;
    }

    [Route("/manage/applications")]
    public async Task<IActionResult> Index() {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        var identfiers = await manager.ListAsync()
            .SelectAwait(application => manager.GetClientIdAsync(application)).ToListAsync();

        return View(identfiers);
    }

    [HttpGet("/manage/applications/create")]    
    public IActionResult Create() {
        return PartialView("AddApp");
    }

    [HttpPost("/manage/applications/save")]
    public async Task<IActionResult> Save(AppModel model) {
        if (ModelState.IsValid) {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();            

            if (await manager.FindByClientIdAsync(model.ClientID) == null) {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor {
                    ClientId = model.ClientID,
                    ClientSecret = model.ClientSecret,
                    ConsentType = model.ConsentType == true ? ConsentTypes.Explicit : ConsentTypes.Implicit,
                    DisplayName = model.DisplayName,
                    PostLogoutRedirectUris = {
                        new Uri(model.LogoutUri)
                    },
                    RedirectUris = {
                        new Uri(model.RedirectUri)
                    },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Logout,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "api"
                    },
                    Requirements =
                    {
                        Requirements.Features.ProofKeyForCodeExchange
                    }
                });
                
                return Json(Result.Success());
            } else {
                #nullable disable
                var app = context.Applications.FirstOrDefault(p => p.ClientId == model.ClientID);
                app.ClientId = model.ClientID;

                if (!string.IsNullOrEmpty(model.ClientSecret)) {
                    app.ClientSecret = model.ClientSecret;
                }

                app.ConsentType = model.ConsentType == true ? ConsentTypes.Explicit : ConsentTypes.Implicit;
                app.DisplayName = model.DisplayName;
                app.PostLogoutRedirectUris = model.LogoutUri;
                app.RedirectUris = model.RedirectUri;

                context.Update(app);

                await context.SaveChangesAsync();

                return Json(Result.Success());

            }
        }

        return PartialView("AddApp", model);
    }

    [HttpGet("/manage/applications/edit")]
    public async Task<IActionResult> Edit(string id) {
        #nullable disable
        await using var scope = _serviceProvider.CreateAsyncScope();
        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        var app = context.Applications.FirstOrDefault(p => p.ClientId == id);

        var data = new AppModel {
            Id = app.Id,
            ClientID = app.ClientId,
            ConsentType = app.ConsentType == ConsentTypes.Explicit ? true : false,
            DisplayName = app.DisplayName,
            LogoutUri = app.PostLogoutRedirectUris,
            RedirectUri = app.RedirectUris
        };

        return PartialView("AddApp", data);
    }
}
