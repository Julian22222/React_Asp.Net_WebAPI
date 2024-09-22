using System.Threading;
using System;
using api.Data;   //connect ApplicationDbContext ,from Data folder
using Microsoft.EntityFrameworkCore;
using api.Interfaces;   //to work with Interfaces
using api.Repository;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using api.Service;   //to work with database


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();  //<--we are telling to .NET Core that we are using Web API 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//To Use NewtonsoftJson extensions, we need to write this code -->
builder.Services.AddControllers().AddNewtonsoftJson(options =>{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;  // write this code to prevent object cycles
});


//make sure you put this code before --> var app = builder.Build();  otherwise it won't work
//ApplicationDBContext <-- DBContext file that we created in Data folder to connect to DB
builder.Services.AddDbContext<ApplicationDBContext>(options=>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});




// builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<BookStoreContext>(); //<--use this code if you are planning to use Usernames, Passwords, LogIn, SignUp in yur App (it has already build in properties), it has standard AspNetUsers table. if we don't want to add any extra properties to AspNetUsers table, to work With Identity Core we need to configure Identity to work with database
builder.Services.AddIdentity<AppUser,IdentityRole>(options => {
  //here we configure all the settigs for Identity framework, if we don't want to use default settings
  //////Configure the password complexity (User Registration), we can customize default password settings -->
    options.Password.RequireDigit = true;  //restrictions for password
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;  //special symbol
    options.Password.RequiredLength = 5;
}).AddEntityFrameworkStores<ApplicationDBContext>();  //<--use this code if you are planning to use Usernames, Passwords, LogIn, SignUp in yur App. Use this code if you are planning to ADD some properties to AspNetUsers table, ( we use Class ApplicationUser with added properties) to work With Identity Core we need to configure Identity to work with database
//AddIdentity <-- will get all the feature that are available in Identity framework core
//IdentityUser <--is a table that already build in Identity framework, to work with a user we insert this table
//AppUser <-- created Model class with added properties to AspNetUsers table 
//IdentityRole <--is a table that already buildIn in Identity framework, to work with roles, we can extend the User Roles but you don't have to
//to connect or to work with our database we write--> .AddEntityFrameworkStores<BookStoreContext>();
//ApplicationDNContext <--our database name




//Here we make our Scheme in order to have JWT
builder.Services.AddAuthentication(options =>{
options.DefaultAuthenticateScheme = 
options.DefaultChallengeScheme = 
options.DefaultForbidScheme = 
options.DefaultScheme = 
options.DefaultSignInScheme = 
options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;  //this will set all defaults for us, all Schemes above
}).AddJwtBearer(options=>{  //Here we adding JWT
    options.TokenValidationParameters = new TokenValidationParameters(){  //TokenValidationParameters comes with JwtBearer, <-- downloaded Nuget Package 
    ValidateIssuer = true,
    ValidIssuer = builder.Configuration["JWT:Issuer"],  //Issuer is a Server URL from appsettings.json, assign values from appsettings.json to the variables
    ValidateAudience = true,
    ValidAudience = builder.Configuration["JWT:Audience"],  //Audience is the Users , who use our App, assign values from appsettings.json to the variables
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))  //SigningKey must be hidden!!!! , it is Secret key. Assign values from appsettings.json to the variables, Here we use a form of encryption --> we convert data from appsettings.json file String (JWT:SigningKey) --> to Bytes 
    };
});




//To use Dependency Injection, All Repository Classes and Interfaces MUST be HERE!!!!
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ITokenService,TokenService>();


var app = builder.Build();   //app is going to control http request pipeline

// Configure the HTTP request pipeline.  / Middlewares
if (app.Environment.IsDevelopment())
{
    //Swagger will be launched only in Development Environment
    app.UseSwagger();
    app.UseSwaggerUI();
}


//We can add this line To handle the errors -->All error will be sent to this URL --> ErrorHandling/ProcessError  //(Where ErrorHandling - ErrorHandlingController.cs and ProcessError - Method name)
app.UseExceptionHandler("/ErrorHandling/ProcessError");  //Errors will be redirected to URL--> /ErrorHandling/ProcessError 

app.UseHttpsRedirection();



//These 2 lines are used for Identity Framework Core (LogIn ,LogOut,SignIn, etc.)
//the order matters
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();




//Write before --> app.Run();
//If we don't add this code the SWAGGER won't work
app.MapControllers();

app.Run();
