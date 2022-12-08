using System.ComponentModel.DataAnnotations;

namespace AuthServer.Models;

public class ChangeModel {

    #nullable disable
    [Required]
    public string UserID { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password & konfirmasi tidak sesuai")]
    public string Confirm { get; set; }
}