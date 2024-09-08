using Microsoft.AspNetCore.Identity;

namespace api.Models;

public class AppUser : IdentityUser  //inherit from Identityuser , then we have access to all defaults and add our own properties
{
    public int Id { get; set; }
    public string UserName { get; set; }

    public string Email { get; set; }
}
