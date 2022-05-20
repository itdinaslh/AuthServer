using System.ComponentModel.DataAnnotations;

namespace AuthServer.Helpers;

public class AppModel {
    #nullable disable
    public string Id { get; set; }
    
    [Required(ErrorMessage = "Client ID is required")]
    public string ClientID { get; set; }
    
    public string ClientSecret { get; set; }

    public bool ConsentType { get; set; } = true;

    [Required(ErrorMessage = "Display Name is missing")]
    public string DisplayName { get; set; }

    [Required(ErrorMessage = "Define Logout URI")]
    public string LogoutUri { get; set; }

    [Required(ErrorMessage = "Define Redirect URI for this app")]
    public string RedirectUri { get; set; }
}
