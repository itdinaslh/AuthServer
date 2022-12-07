using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthServer.Data;

public class ApplicationUser : IdentityUser { 
    [MaxLength(50)]
    public string? FullName { get; set; }

    public int? InjectID { get; set; }
}