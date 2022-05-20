using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;

namespace AuthServer.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
    #nullable disable
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext>  options) : base(options) { }

    public DbSet<OpenIddictEntityFrameworkCoreApplication> Applications { get; set; }
    
}
