using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;     //<-- access to Item and Comment Model
using Microsoft.EntityFrameworkCore;    //to work with database
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace api.Data;


////we can inherit this class from DbContext class to work with database -->
////We inherit from DbContext if we not planning to use UserNames, Passwords, LogIn,SignUp,Security functions in our App
// public class ApplicationDBContext : DbContext


//or we can inherit this class from IdentityDbContext class to work with Identity
//IdentityDbContext class is inherited from DbContext class under the hood

//  public class BookStoreContext : IdentityDbContext  <--use this code if you are planning to use Usernames, Passwords, LogIn, SignUp in yur App (it has already build in properties), it has standard AspNetUsers table, use this approach if we don't add any extra properties to AspNetUsers table



public class ApplicationDBContext : IdentityDbContext<AppUser>   //<-- use this code if you are planning to use Usernames, Passwords, LogIn, SignUp in your App and planning to ADD some extra properties to AspNetUsers table, will create all needed tables for users and security automatically in our database, we use Class AppUser with added properties
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


    //override onc + Tab , <--to have a taplate
    // protected override void OnModelCreating(ModelBuilder builder)
    // {
    //     base.OnModelCreating(builder);

    //     List<IdentityRole> roles = new List<IdentityRole>(){     //Creating Identity Roles

    //     new IdentityRole(){
    //         Name = "Admin",
    //         NormalizedName = "ADMIN"   //NormalizedName means it is Capitalised
    //     },
    //     new IdentityRole(){
    //         Name = "User",
    //         NormalizedName = "USER"   //NormalizedName means it is Capitalised
    //     },
    //     };


    //     builder.Entity<IdentityRole>().HasData(roles);    //Adding roles to IdentityRole
    // }
}
