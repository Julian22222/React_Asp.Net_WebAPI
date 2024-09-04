using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;   //Can you Task with async await , and to use Task
using Microsoft.EntityFrameworkCore;  //to use ToListAsync method, SaveChangesAsync(), FindAsync(id); and other asyn methods
using System.ComponentModel.DataAnnotations;  //used for Atributes
 
namespace api.Dtos.Comment;

public class CreateCommentDto
{

    //Server side Data Validation
    [Required]
    [MinLength(5, ErrorMessage = "Title must be 5 characters")]  //Min length = 5 , otherwise will show this msg
    [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]  //Max lenght = 280
    public string Title { get; set; } = string.Empty;

    [Required]
    [MinLength(5, ErrorMessage = "Content must be 5 characters")]  //Min length = 5 , otherwise will show this msg
    [MaxLength(280, ErrorMessage = "Content cannot be over 280 characters")]  //Max lenght = 280
    public string Content { get; set; } = string.Empty;
}
