using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Item
{
    public class UpdateItemRequestDto
    {

    [Required]
    [MaxLength(30, ErrorMessage = "Name cannot be over 30 characters")]
    public string Name { get; set; } = string.Empty;  //<--string.Empty == "";

    [Required]
    [MaxLength(20, ErrorMessage = "Type cannot be over 20 characters")]
    public string Type { get; set; }

    [Required]
    [MaxLength(500, ErrorMessage = "Description cannot be over 500 characters")]
    public string Description { get; set; } = "";


    [Required]
    public int Qty { get; set; } = 0;


    // if we are dealing with money, can have only 18 digits and only 2 decimal places
    // [ColumnAttribute(TypeName = "decimal(18,2)")]
    [Required]
    [Range(1, 10000000)]
    public decimal Price { get; set; }

        
    }
}