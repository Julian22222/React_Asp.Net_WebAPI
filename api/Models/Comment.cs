using System;
namespace api.Models;

public class Comment
{
    public int Id { get; set; }   //Primary Key
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public int? ItemId { get; set; }  //foreign key of Items Table , each comment should be referenced to some Item

    //Linking two DB tables
    public Item? Item { get; set; }    //field is optional
}
