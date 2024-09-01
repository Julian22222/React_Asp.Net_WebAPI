using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;     //<-- access to Item and Comment Model
using Microsoft.EntityFrameworkCore;    //to work with database

namespace api.Data;

public class ApplicationDBContext : DbContext
{

    //constructor --> ctor + tab
    //: base(dbContextOptions)  <-- allow us inherit from --> :DbContext(), but we can't write like this
    //Therefore we have to write --> :base(dbContextOptions) , it will allow us to pass our DBContext(in our example - dbContextOptions) into inherited DbContext --> :DbContext (to line 10)
    public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
        
    }



    //adding tables to our DB, Items - Name of the table, using Item as a Class
    public DbSet<Item> Items { get; set; }


    //adding table to our DB, Comments -  Name of the table, using Comment as a template
    public DbSet<Comment> Comments { get; set; }
}
