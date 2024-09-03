using System.Threading;
using System;
using api.Data;   //connect ApplicationDbContext ,from Data folder
using Microsoft.EntityFrameworkCore;
using api.Interfaces;   //to work with Interfaces
using api.Repository;   //to work with database


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


//To use Dependency Injection, All Repository Classes and Interfaces MUST be HERE!!!!
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();


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


app.UseAuthorization();

app.MapControllers();




//Write before --> app.Run();
//If we don't add this code the SWAGGER won't work
app.MapControllers();

app.Run();
