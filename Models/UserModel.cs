namespace AuthServer.Models;

public class UserModel {
    #nullable disable
    public string FullName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Roles { get; set; }

    public string UserName { get; set; }

    public string PhoneNumber { get; set; }
}