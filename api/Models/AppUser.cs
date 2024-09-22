using Microsoft.AspNetCore.Identity;

namespace api.Models;

public class AppUser : IdentityUser  //inherit from Identityuser , then we have access to all defaults and add our own properties
{

    //Here we inherit this Class from IdentityUser, where already AspNetUsers table -->has UserName property
    public int Id { get; set; }

    // public string UserName { get; set; }  <-- don't need because it is already build in property in IdentityUser class

    public string Email { get; set; }
}
