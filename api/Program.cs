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
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureKeyVault;  ////import installed nuget package (if you installed any nuget packages , you need this to use packages here)-> AddRazorRuntimeCompilation();, to use ClientSecretCredential



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


if(builder.Environment.IsDevelopment()){

    //make sure you put this code before --> var app = builder.Build();  otherwise it won't work
    //ApplicationDBContext <-- DBContext file that we created in Data folder to connect to DB
    builder.Services.AddDbContext<ApplicationDBContext>(options=>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
}



if(builder.Environment.IsProduction()){

//make sure you put this code before --> var app = builder.Build();  otherwise it won't work
//ApplicationDBContext <-- DBContext file that we created in Data folder to connect to DB
// builder.Services.AddDbContext<ApplicationDBContext>(options=>{
    //this 3 lines --> to use .env file
    // DotNetEnv.Env.Load();
    // var connection_string = Environment.GetEnvironmentVariable("AZUREDB_ConnectionStrings");
    // options.UseSqlServer(connection_string);



var keyVaultURL = builder.Configuration.GetSection("KeyVault:KeyVaultURL");             //getting info from appsettings.json
var keyVaultClientId = builder.Configuration.GetSection("KeyVault:ClientId");           //getting info from appsettings.json
var keyVaultClientSecret = builder.Configuration.GetSection("KeyVault:ClientSec");  //getting info from appsettings.json
var keyVaultDirectoryID = builder.Configuration.GetSection("KeyVault:DirectoryID");    //getting info from appsettings.json

//this allow us to authenticate us in Azure ID, to prove that we are who we are, to access to resources that we want in Azure Portal   
var credential = new ClientSecretCredential(keyVaultDirectoryID.Value!.ToString(), keyVaultClientId.Value!.ToString(), keyVaultClientSecret.Value!.ToString());

//adding Azure Key Vault, use all our values to access Azure Key Vault
builder.Configuration.AddAzureKeyVault(keyVaultURL.Value!.ToString(), keyVaultClientId.Value!.ToString(), keyVaultClientSecret.Value!.ToString(), new DefaultKeyVaultSecretManager());

//the tool that go and create that connectionString with Azure Key Vault
var client = new SecretClient(new Uri(keyVaultURL.Value!.ToString()), credential);


builder.Services.AddDbContext<ApplicationDBContext>(options =>
options.UseSqlServer(client.GetSecret("ProdConnction").Value.Value.ToString())); //inject DB context into our app
//ProdConnection <-- is the name thet we created in Key Vault in Azure Portal to keep our Secrets in secret (our connection string to database) 


}



builder.Services.AddCors(opt=>{
    opt.AddPolicy(name: "ItemAPi", builder=>{
        builder.WithOrigins("http://localhost:5113")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials(); 
    });
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
// builder.Services.AddAuthentication(options =>{
// options.DefaultAuthenticateScheme = 
// options.DefaultChallengeScheme = 
// options.DefaultForbidScheme = 
// options.DefaultScheme = 
// options.DefaultSignInScheme = 
// options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;  //this will set all defaults for us, all Schemes above
// }).AddJwtBearer(options=>{  //Here we adding JWT

// //These 2 lines --> to use .env file
// // DotNetEnv.Env.Load(); 
// // var key = Environment.GetEnvironmentVariable("JWT_SigningKey");


//     options.TokenValidationParameters = new TokenValidationParameters(){  //TokenValidationParameters comes with JwtBearer, <-- downloaded Nuget Package 
//     ValidateIssuer = true,
//     ValidIssuer = builder.Configuration["JWT:Issuer"],  //Issuer is a Server URL, if we want to get Issuer from from appsettings.json use this line, assign values from appsettings.json to the variables
//     ValidateAudience = true,
//     ValidAudience = builder.Configuration["JWT:Audience"],  //Audience is the Users , who use our App, assign values from appsettings.json to the variables, If we want to use Audience from appsettings.json use this line
//     ValidateIssuerSigningKey = true,
//     IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))  ////SigningKey must be hidden!!!! , it is Secret key. Assign values from appsettings.json to the variables, Here we use a form of encryption --> we convert data from appsettings.json file String (JWT:SigningKey) --> to Bytes 
    
//     //.env file
//     // IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key)) 
//     };
// });




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



if (app.Environment.IsProduction())
{
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
