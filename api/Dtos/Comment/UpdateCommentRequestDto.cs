// using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment;

public class UpdateCommentRequestDto
{

    [Required]
    [MinLength(5, ErrorMessage = "Title must be 5 characters")]  //Min length = 5 , otherwise will show this msg
    [MaxLength(280, ErrorMessage = "Title cannot be over 280 characters")]  //Max lenght = 280
    public string Title { get; set; } = string.Empty;

    [Required]
    [MinLength(5, ErrorMessage = "Content must be 5 characters")]  //Min length = 5 , otherwise will show this msg
    [MaxLength(280, ErrorMessage = "Content cannot be over 280 characters")]  //Max lenght = 280
    public string Content { get; set; } = string.Empty;
}
