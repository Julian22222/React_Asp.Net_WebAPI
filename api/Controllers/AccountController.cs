// // using System;
// // using System.Collections.Generic;
// // using System.Linq;
// // using System.Threading.Tasks;   //Can you Task with async await , and to use Task
// // using Microsoft.EntityFrameworkCore;  //to use ToListAsync method, SaveChangesAsync(), FindAsync(id); and other asyn methods


// using api.Dtos.Account;
// using api.Models;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;  //needs for ControllerBase

// namespace api.Controllers;

// [Route("api/account")]   //<-- all the Routes will start with this (For Account)
// [ApiController]  //this attribute indicates (tells to compilator) that we are going to use API Controller in this Class
// public class AccountController :ControllerBase
// {

//     //UserManager - is buildin in Identity framework, needs for SignUp 
//     //SignInManager is buildIn in Identity framework, needs for LogIn, Log Out, and we can check if User Loged In or Not 
//     //These 2 Manager are very importan to work with user details and to handle Authentication and Authorisation

//     //IdentityUser <--It is a User Class (is buildin in Identity framework)
//     //_userManager <-- is a name of User Class (can be any name)

//     //  private readonly UserManager<IdentityUser> _userManager;  <--use this code if we use standard AspNetUsers table, if we don't add any properties to AspNetUsers table.
//     private readonly UserManager<AppUser> _userManager;    //UserManager is used for Sign Up, Change password and etc. (all operations that are specific for particular user --> these are available in this _userManager ), Here we creating variable for SignUp and ChangePassword, to interact with database's AspNetUsers table
//     // AppUser --> is User Class that we created, where we added extra porperties to standart AspNetUsers table

//     // constructor, here we use dependency injection, application will resolve AccountRepository automatically
//     // because we have written the code in our -Program.cs file -> (line 50) ->  builder.Services.AddScoped<AccountRepository, AccountRepository>();
//     public AccountController(UserManager<AppUser> userManager)
//     {
//         _userManager = userManager;    //using _userManager -> we have acces to AspNetUsers table, when SignUp
//     }


//     // public AccountRepository(UserManager<IdentityUser> userManager){
//     //     _userManager = userManager;
//     // }   <---use this code if we use standard AspNetUsers table, if we don't add any properties to AspNetUsers table




//     //End-point for this method will be --> [Route("api/account")] + /register
//     [HttpPost("register")]
//     public async Task <IActionResult> Register([FromBody] RegisterDto registerDto){  //we need [FromBody], because our Data will be in form of JSON, we not passing the data through URL, we passing the data in the--> body of HTTP when user Register. //RegisterDto will be a template Form to fill to Add new object to DB, will appear in Swagger Schemas


//         //wrap everything in try catch, because are a lot of different server errors that can happen when you use Usermanager and CreateAsync( user, "password"), we are going to validate user Password complexity and if we don't anything to catch these errors it will cause a lot of problems 
//         try{
//             if(!ModelState.IsValid){  //Model State valiation, it will catch all the errors within our DTO Class
//                 return BadRequest(ModelState);
//             }
//             else
//             {
//                 var appUser = new AppUser(){   //assigning received data from -> registerDto to appUser Class
//                     UserName = registerDto.Username,
//                     Email = registerDto.EmailAddress
//                 };

//                 //CreateAsync Method always return an object, that will have a properties that we can check
//                 var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

//                 if(createdUser.Succeeded){   //checking have we created the new User in DB and is it Succesfull 
//                     var roleResult = await _userManager.AddToRoleAsync(appUser, "User");  //It is very similar to CreateAsync method but it is for Roles --> AddToRoleAsync Method will return an object, that will have a properties that we can check
//                     //anybody who SignsUp through --> register end-point, will be assign to User Role
                
    
//                     //for Admin we can add it manually or create another end-point which will allow to assign as an Admin with Admin Role

//                     if(roleResult.Succeeded){  //checking is roleResult object is not null and contains some data in its properties
//                         return Ok("User created");
//                     }
//                     else
//                     {
//                         return StatusCode(500, roleResult.Errors);  // will send Status Code 500 and Server Error 
//                         //return BadRequest(ModelState);  //<-- the same
//                     }
//                 }
//                 else
//                 { //if createdUser not Successful
//                     return StatusCode(500, createdUser.Errors);  /// will send Status Code 500 and Created User not Successfuly Loged-In
//                 }
//             }
//         }
//         catch(Exception err){  //all other exceptions and errors will show 
//             return StatusCode(500, err);
//         }
//     }



// }
