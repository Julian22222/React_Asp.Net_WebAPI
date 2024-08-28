using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models;

public class Item
{
    public int Id { get; set;}
    // public IFormFile? Image { get; set;}

    [Required]
    public string Name { get; set; } = "";

    // [Required]
    // public string Type { get; set; }

    [Required]
    public string Description { get; set; } = "";

    // [Required]
    // public decimal Price { get; set; }
}
