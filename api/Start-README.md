# To start the WebAPI project:

- we delete ASP.NET_WEB_API.generated.sln //<--file with Visual Studio logo (purple)

```C#
  //If don't delete the file , show this error-->
Specify which project or solution file to use because this folder contains more than one project or solution file.
```

1. in terminal --> dotnet new list
2. In terminal --> dotnet new webapi
3. we delete ASP.NET_WEB_API.generated.sln //<--file with Visual Studio logo (purple)
4. In terminal --> dotnet watch run

### How to Check what .NET version we are using

- Go to api.csproj file in the root of your Project Folder
- Check line 4 -->

```C#
  <PropertyGroup>
    <TargetFramework>net7.0</TargetFramework>
```

# Install packages from Nuget Package

#### Dowload --> NuGet Gallery //<-- by pcisio

- Then In Command Palette put (ctr + shift + p )-->

```bash
NuGet:Open NuGet Gallery
```

- add your .NET version for a package and press "+" in api.csproj section
- to delete some packages we can press "-" in api.csproj section

#### To install nuget packages with different version , not the latest version we put: (EXMAPLE) Write in terminal -->

```C#
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0
```

##### We need to install following packages

```C#
Microsoft.EntityFrameworkCore.SqlServer    //<--if we wan to have SQL Server in the B-End
Microsoft.EntityFrameworkCore.Tools
Microsoft.EntityFrameworkCore.Design
```

# We need to create Database in SSMS

### SQL Server Management Studio (SSMS)

- It is an Application to work with local DB and Web DB

- We are creating Database in SSMS (Create a name for local DB)
- Create Models
- Create Data/ApplicationDBContext.cs file to connect to our DB
- Create Controllers/ Controller file
- in Program.cs file we add DB Connection String, to connect to correct DB (usibg DB name and password) (-->See line 20)

```C#
//The DB ConnectionString allow to connect with correct DB and create all need tables and properties

builder.Services.AddDbContext<ApplicationDBContext>(options=>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
```

and

```C#
builder.Services.AddControllers();


//Write before --> app.Run();
//If we don't add this code the SWAGGER won't work
app.MapControllers();
```

- Then put in terminal --->

```C#
//this command uses connection string and Data/DBContext file

//  dotnet ef migrations add (AnyMigrationsName)
dotnet ef migrations add Init     //<--this command allow to add tables and properties to the correct database, Create Migrations Folder in the our App

dotnet ef database update     ////<-- update the DB
```

# dotnetenv

- download dotnetenv package from Nuget Packages
- to use --> .env secret files

- the to use files from .env we need to write

```C#
DotNetEnv.Env.Load();


//this option to use in Program.cs file
//LOCALDB_CONNECTION_STRING <-- name of you .env variable
var con_string = Environment.GetEnvironmentVariable("LOCALDB_CONNECTION_STRING");
--> use con_string






//this option for any other classes
private readonly IConfiguration _configuration;  //declare configuration //<--using IConfiguration we can have access to appsettings.json and secrets and .env file


//in constructor
 public MyBookStoreWebDbContext(IConfiguration configuration){
_configuration = configuration;  //<--needs to connects to appsettings.json and secret files
}


var secret = _configuration["LOCALDB_CONNECTION_STRING"];

```
