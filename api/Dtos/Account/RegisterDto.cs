using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Account;

public class RegisterDto
{   

    [Required]  //Require to put the username in the Form
    public string? Username { get; set; }  //optional property

    [Required]
    [EmailAddress]  //.NET Core automatically does the email address validation
    public string? EmailAddress { get; set; }  //optional property

    [Required]
    //can add Regex to implement password validation
    public string? Password { get; set; }  //optional property
}
