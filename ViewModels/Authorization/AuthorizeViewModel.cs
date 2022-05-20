using System.ComponentModel.DataAnnotations;

namespace AuthServer.ViewModels.Authorization;

public class AuthorizeViewModel {
    #nullable disable
    [Display(Name = "Application")]
    public string ApplicationName { get; set; }

    [Display(Name = "Scope")]
    public string Scope { get; set; }
}