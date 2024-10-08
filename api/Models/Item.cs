using System.ComponentModel.DataAnnotations.Schema;
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
    public string Name { get; set; } = string.Empty;  //<--string.Empty == "";

    [Required]
    public string Type { get; set; }

    [Required]
    public string Description { get; set; } = "";


    [Required]
    public int Qty { get; set; }


    // if we are dealing with money, can have only 18 digits and only 2 decimal places
    [ColumnAttribute(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }



    //one to many relationship, we use Comment Model in here, That you can leave many comments under one Item
    //Here we are linking two DB tables
    public List<Comment> Comments { get; set; } = new List<Comment>();

    //public long MarketCApPrice {get; set;}   //<-- long allow us to use huge numbers--> trilions
}
