// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;   //Can you Task with async await , and to use Task
// using Microsoft.EntityFrameworkCore;  //to use ToListAsync method, SaveChangesAsync(), FindAsync(id); and other asyn methods


using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;  //needs for ControllerBase

namespace api.Controllers;

[Route("api/account")]   //<-- all the Routes will start with this (For Account)
[ApiController]  //this attribute indicates (tells to compilator) that we are going to use API Controller in this Class
public class AccountController : ControllerBase
{

    //UserManager - is buildin in Identity framework, needs for SignUp 
    //SignInManager is buildIn in Identity framework, needs for LogIn, Log Out, and we can check if User Loged In or Not 
    //These 2 Manager are very importan to work with user details and to handle Authentication and Authorisation

    //IdentityUser <--It is a User Class (is buildin in Identity framework)
    //_userManager <-- is a name of User Class (can be any name)

    //  private readonly UserManager<IdentityUser> _userManager;  <--use this code if we use standard AspNetUsers table, if we don't add any properties to AspNetUsers table.
    private readonly UserManager<AppUser> _userManager;    //UserManager is used for Sign Up, Change password and etc. (all operations that are specific for particular user --> these are available in this _userManager ), Here we creating variable for SignUp and ChangePassword, to interact with database's AspNetUsers table
    // AppUser --> is User Class that we created, where we added extra/additional porperties to standart AspNetUsers table

    
    //SignInManager is used to check the password
    private readonly SignInManager<AppUser> _signInManager;


    private readonly ITokenService _tokenService;


    // constructor, here we use dependency injection, application will resolve AccountRepository automatically
    // because we have written the code in our -Program.cs file -> (line 50) ->  builder.Services.AddScoped<AccountRepository, AccountRepository>();
    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;    //using _userManager -> we have acces to AspNetUsers table, when SignUp
        _tokenService = tokenService;
        _signInManager = signInManager;
    }


    // public AccountRepository(UserManager<IdentityUser> userManager){
    //     _userManager = userManager;
    // }   <---use this code if we use standard AspNetUsers table, if we don't add any properties to AspNetUsers table



    [HttpPost("login")]  //End-point for this method will be --> [Route("api/account")] + /login
    public async Task<IActionResult> Login(LoginDto loginDto){ //use LoginDto Form to LogIn

        //it is better to use try catch 
        //can use --> public async Task<IActionResult> Login([FromBody] LoginDto loginDto){..}

        if(!ModelState.IsValid){  //if LogIn form is field incorrectly
            return BadRequest(ModelState);
        }

        //Find the User in DB
        //We can find by Username or by Email
        var user = await _userManager.Users.FirstOrDefaultAsync(x=> x.UserName == loginDto.Username.ToLower());
    
        if(user == null){ //if we couldn't find the User
            return Unauthorized("Invalid Username!");
        }

        //Check is the password is correct for this user
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false); //false --> lockoutOn Failure, if you enter passwords unsuccessfully few times account will be blocked, if you will put true --> it will give lots of issues and erros, it needs to be adjusted in Program.cs file (how many attempts is alllowed to use before it lock the account)
    
        if(!result.Succeeded){  //if you didn't logedIn Successfully
            return Unauthorized("Username not found and/or password incorrect");  //we don't want explicitly tell what is an issue, it will be easier to hackers to get the Password
        }

        
        //if User Loged In Successfully, return this Form (NewUserDto)
        return Ok(
            new NewUserDto(){  
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            }
        );

    }





    //End-point for this method will be --> [Route("api/account")] + /register
    [HttpPost("register")]
    public async Task <IActionResult> Register([FromBody] RegisterDto registerDto){  //we need [FromBody], because our Data will be in form of JSON, we not passing the data through URL, we passing the data in the--> body of HTTP when user Register. //RegisterDto will be a template Form to fill to Add new object to DB, will appear in Swagger Schemas


        //wrap everything in try catch, because are a lot of different server errors that can happen when you use Usermanager and CreateAsync( user, "password"), we are going to validate user Password complexity and other validation and if we don't make anything to catch these errors it will cause a lot of problems 
        try{
            if(!ModelState.IsValid){  //Model State valiation, it will catch all the errors within our DTO Class
                return BadRequest(ModelState);
            }
            else
            {

                //if ModelState == true
                var appUser = new AppUser(){   //assigning received data from -> registerDto to appUser Class
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                //UserManager has method -->CreateAsync Method and it always returns an object, that will have a properties that we can check -> if it succesfull or not
                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if(createdUser.Succeeded){   //checking have we created the new User in DB and is it Succesfull 
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");  //It is very similar to CreateAsync method but it is for Roles --> AddToRoleAsync Method will return an object, that will have a properties that we can check
                    //anybody who SignsUp through --> register end-point, will be assign to User Role
                
    
                    //for Admin we can add it manually or create another end-point which will allow to assign as an Admin with Admin Role

                    if(roleResult.Succeeded){  //checking is roleResult object is not null and contains some data in its properties
                        // return Ok("User created");
                        return Ok(
                            new NewUserDto(){  //we return this form, when User created Succesfully
                                //Claims --> appUser.UserName, appUser.Email

                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);  // will send Status Code 500 and Server Error 
                        //return BadRequest(ModelState);  //<-- the same
                    }
                }
                else
                { //if createdUser not Successful
                    return StatusCode(500, createdUser.Errors);  /// will send Status Code 500 and Created User not Successfuly Loged-In
                }
            }
        }
        catch(Exception err){  //all other exceptions and errors will show 
            return StatusCode(500, err);
        }
    }



}
