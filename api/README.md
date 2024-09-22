[Web API Docs with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-8.0&WT.mc_id=dotnet-35129-website&tabs=visual-studio-code)

# Connection Fron-end project to the back-end

Swagger and React can be used at the same time. That's the best part about swagger. You have frontend React plus Swagger to build when you have to React infrastructure.

# Swagger Documentation

- Is called Swagger or Swagger Interface
- It is a library that interacts with Interface for special visualisation of API
- Show all your API routes

- Heading in Swagger interface is taken from Controller Names (Comment or Item) --> From CommentController , ItemController

#### query params are case sensitive --> doesn't matter is it in capital or small letter written

#### Don't name your controller methods with word "Async" or you will get error 500 no route matches when you will try to use CreatedAtAction on this method. The name of function should not end with Async.

# DTO - Stands for Data Transfer Object (90% of DTO will be --> request, response)

DTO help to trim the object and pass only needed data-->

```C#
//For example if we have this object, but we want to have username and pass only username to somewhere
{
    username: "Jim",
    password: "Space1234"
}
```

---

### DTO is used --> when we want to modefy some properies in already existing Class--> we create similar class and delete or Add some properties that we don't need or need extra properties (Work together with a Mapper)

- DTO Classes are used to show needed properties to the User. After we receive the Data from DB , then we can convert that Data to needed Format to show to the User
- DTO Classes are used to use DTO Format as a tamplate for a User to fill a Form in Front-End / fill fields in Swagger --> then that Data we can convert to DataBase data type Format using DTO Classes and Add the Data to Database
- it is creating new object but with modified properies (example: Item and ItemDto)
- We create Dtos folder and put all new modefied Models files inside that folder
- DTO Models are used only to show some Data to the Clinet or to make a Template to fill the Form, after thet converting it to DB Format and send it to the DB, we can't send DTO to DB
- if we want to show some details but we don't need full object of all properties
- For example we don't want to receive the Comments for an Item, then we create the same Item object but without Comments property
- DTO and Mapper work together
- In WEB API , All DATA VALIDATION we insert in DTO Classes. Not in Model Classes !!! --> Because it will apply Data validation globally, but we don't want that

##### SEE --> layout.tldr file

##### See --> DTO-README.md file

### Mapper (work together with DTO)

- Mapper Class is static Class where we convert one data type to another data type
- we create Mappers folder and New Model (where some properties are removed from The same Class).
  As example in Item Class we have all needed properties. But ItemDto Class will use the same properties but some properties will be removed comparing with Item Class - we are trimm out some properties from Item Class. (because somethimes we don't need some Data, or we don't want to show all the properties to the user etc.).
- in ItemMappers.cs file we have methods that are assigning different classes together --> (For example) Item Class with itemDto Class
- Mappers Classes have static methods that we can use for trimming out some properties from the object.

- Also, We can have 2 AutoMapper extensions

```C#
AutoMapper  //<-- by Jimmy Bogard
AutoMapper.Extensions.Microsoft.DependencyInjection  //<-- by Jimmy Bogard
```

then in Program.cs file write-->

```C#
//write after this command
//builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(MappingConfig));
```

---

# Async

- When we use async --> it always goes with Task
- When we use async --> we HAVE to return something from this function

```C#
//Task<Item> - it is return data type

public async Task<Item> GetAll(){
    ......
}
```

# Interfaces

- Interfaces is similar to Repository Classes,
- Interfaces are inherited by Repository Classes and have the same methods and arguments as Repository Class that inherits this Interface
- Interfaces have only Methods name with arguments but all logic of these methods are in Repository Class
- Interfaces are easier to implement and use in Application
- Then we use Interfaces to interact with DB in different classes, we import Interface

# To Get The Comments in GET request for All Items

- We change Itemrepository.cs file (line 49 and 57)

```C#
 return await _context.Items.Include(x=>x.Comments).ToListAsync();


return await _context.Items.Include(x=>x.Comments).FirstOrDefaultAsync(m => m.Id == id);
```

- change ItemMappers.cs file (line 27)

```C#
 Comments = item.Comments.Select(x => x.ToCommentDto()).ToList(),
```

- Then we need to install 2 Nuget packages:

```C#
Newtonsoft.Json   //<-- by James newton-King

Microsoft.AspNetCore.Mvc.NewtonsoftJson   //<--by Microsoft ASP.NET Core MVC

//Also, We can use System.Text.JSON, It is built in to ASP.NET, it is faster and uses less memory comparing to Newtonsoft.JSON--> System.Text.JSON
//it is the same as --> Newtonsoft.JSON

// System.Text.Json can handle circular references.You can control its behaviour by changing/setting the value of  JsonSerializerOptions.ReferenceHandler to ReferenceHandler.Preserve or some other value as per your requirement.
```

- Then to use this packages in our App we need to write some code in Program.cs file (line 19)

```C#
//To Use NewtonsoftJson extensions, we need to write this code -->
builder.Services.AddControllers().AddNewtonsoftJson(options =>{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;  // write this code to prevent object cycles
});
```

### benefit using newtonsoft instead of using automapper

- Auto mapper does everything for you but it creates errors that are confusing. I make my own mappers with newtonsoft because i can fix them easier. Some people love auto mapper too so it’s personal choice really.

# Data Validation

- Never put Data Validation directly in the Model Classes (Models/Comment.cs and Models/Item.cs files), then it is going to apply it globally, but we don't want that

- Therefore it is another reason to create DTO Classes, to apply all needed Data Validation inside DTO Classes

- ControllerBase is giving us ModelState object for Data Validation in Controller Class
- Each time this actual Controller executes --> it is going to give us the brand new ModelState object and if the ModelState is Not Valid it will trigger Bad Request

# Filtering

```C#
await -context.Items.ToListAsync();
```

- When you getting Data from the database 90% of the time you will get the data in the form of the list

- ToList() --> it is generates SQL , SQL request the Data from Database and it will send a response back to the user. SQL get the Data from DB and send a response back

### How to fileter/search? Hot to Limit? How to add additional logic to database request?

In these case we need to use --> AsQueryable (SEE --> ItemRepository file, line 60)

```C#
var items = await _context.Items.AsQueryable();  //AsQueryable is going to deley the SQL request to the Database, therefore we can have time to filter, to Limit();

items.Where(x=>x.Name == name);  //we can filter

items.Limit(2);  //We can Limit the items of 2 pcs

//after you arranged the data you want from database you can use --> ToList(); --> send SQL to the Database and get the data back

items.ToList();  //To List is used in the end of our logic!
```

### We might need to filter Items by the Name

- There are many ways how we can filter Items by Name (for example by Name), but one of the best ways is to create an object and insert this object to the --> GetAll() Method

```C#
 //This Method have the Route --> [Route("api/items")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query){ //[FromQuery]  <-- get the data from query in the URL, QueryObject <--data type , query <-- the name of this data

        //some code here

        var data = await _iitemRepository.GetAllItems(query); //passing the query to Repository to interact with Database
    }

//We create QueryObject Class in --> Helpers folder
```

- When we Search --> we don't pass info through the Route, End-Point. We are passing data through Query parameters

```C#
//Query parameters is look like this --> they have key-value pairs

https://localhost:5113/api/items?type=phone

//or

https://localhost:5113/api/items?name=xiaomi
```

# Sorting

```C#
var items = await _context.Items.AsQueryable();  //AsQueryable is going to deley the SQL request to the Database, therefore we can have time to filter, sorting, to Limit(); and other addition logic to Database query

items.orderByDescending(x=> x.Name);

items.OrderBy(x=>x.Name);

items.ToList();  //To List is used in the end of our logic!
```

Then -->

For a Sorting we add to --> QueryObject Class in Helpers folder

```C#
public string? SortBy { get; set; } = null;

public bool IsDescending { get; set; } = false;
```

```C#
 //in ItemRepository , we add -->

  if(!string.IsNullOrWhiteSpace(query.SortBy)){

   // query.SortBy.Equals("Name"  <--if  you want sort by Name
            // StringComparison.OrdinalIgnoreCase  <-- in this case we ignore Original / Normal sorting

            if(query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase)){

                 //query.IsDescending <-- can be true or false, coming from query
                //if query.IsDescending == true --> we sort Items in descending order , else ascending
                items = query.IsDescending ? items.OrderByDescending(x=>x.Name) : items.OrderBy(x=>x.Name);
            }
        }
```

# Pagination

- If we have 100 000 objects in our Database and we want to return them all at once, our app will probably crash. This way we have Pagination
- pagination does 2 things:
  - don't return all results at once
  - break them into pages

```C#
//this method will skip first 2 elements in the Database and return all elements apart from first 2 elements
.Skip(2)


//This method will take first 2 elements from Database and return them
.Take(2)

//if we will combine Skip and Take
// .Skip(2).Take(2)  --> then we can create pages using Pagination
```

- We can put pagination directly to -->

```C#
 [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query  //<--directly here)
    {
        //some code

    }
```

but won't do it.

- We will add 2 extra properties in QueryObject Class

```C#
//this is just for learning purposes, Front-end doesn't need this pages

    //for Pagination
    public int PageNumber { get; set; } = 1;

    //how many Items show on one Page
    public int PageSize { get; set; } = 20;
```

- Then in ItemRepository we add this code --> line 88

```C#
 //PageNumber by default = 1 --> (1 - 1) * 20 = 0 <--will Skip O Items
var skipNumber = (query.PageNumber -1) * query.PageSize;    //here we do calculation for Pagination


 //Skip(skipNumber) == 0, Take(query.PageSize) = 20 (by default) <-- will show first 20 Items
return await items.Skip(skipNumber).Take(query.PageSize).ToListAsync();

```

# Web API Identity JWT

- User is going to Log In with ane email and a Password
- we will issue JWT <-- which is string of letters and numbers,--> which will go between logIn Form and a Server
- like encripted password, Server will validate JWT
- JWT has:
  - header
  - payload
  - Encryption (header + payload,secret) <--will encript the header payload and a secret separating them by dot --> "sdfgihdoihsedv397qg7.3c083hc2w90.h302w89h34f23f" (header.payload.signature)

To work wit thses Install some packages: ( ctr + alt + p - write --> use Nuget Gallery)

```C#
//To work with Identity Framework Core, for LogIn, SignUp, etc.
Microsoft.Extensions.Identity.Core  //<--by Microsoft

Microsoft.AspNetCore.Identity.EntityFrameworkCore  //<-by Microsoft

Microsoft.AspNetCore.Authentication.JwtBearer   //<-by Microsoft
```

- Then we need to add a Model for a User, where we will keep the data for a user(such as: password, email etc.)

- Create in Model/ AppUser Class (Here we will keep user details in properties)

- In Data/ApplicationDBContext.cs file --> change inheritance from IdentityDbContext<AppUser> (See --> line 25)

- Then we need to let our Application know that we are using AppUser Class in IdentityDbContext

In Program.cs -->

```C#
// builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>(); //<--use this code if you are planning to use Usernames, Passwords, LogIn, SignUp in yur App (it has already build in properties), it has standard AspNetUsers table. if we don't want to add any extra properties to AspNetUsers table, to work With Identity Core we need to configure Identity to work with database
builder.Services.AddIdentity<AppUser,IdentityRole>(
 //here we configure all the settigs for Identity framework, if we don't want to use default settings
  //////Configure the password complexity (User Registration), we can customize default password settings -->
    options.Password.RequireDigit = true;  //restrictions for password
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;  //special symbol
    options.Password.RequiredLength = 5;
).AddEntityFrameworkStores<ApplicationDBContext>();  //<--use this code if you are planning to use Usernames, Passwords, LogIn, SignUp in your App. Use this code if you are planning to ADD some properties to AspNetUsers table, ( we use Class AppUser with added properties) to work With Identity Core we need to configure Identity to work with database
//AddIdentity <-- build-in, will get all the feature that are available in Identity framework core
//IdentityUser <--is a table that already build in Identity framework, to work with a user we insert this table
//AppUser <-- created Model class with added properties to AspNetUsers table
//IdentityRole <--is a table that already buildIn in Identity framework, to work with roles. We can extend the User Roles but you don't have to
//to connect or to work with our database we write--> .AddEntityFrameworkStores<BookStoreContext>();
//ApplicationDBContext <--our database name
```

Now we need to make our Scheme --> it means are you going to add JWT ? are you going to add cookies ? or are you going to add a mixture of both.

- Here we are not using Cookies , we are using JWT in this project
- We are going to set our Scheme up in order to have JWT

[What is JWT ?](https://dev.to/jaypmedia/jwt-explained-in-4-minutes-with-visuals-g3n#:~:text=JWT%20stands%20for%20JSON%20Web,known%20only%20to%20the%20server.)

To Add the Scheme we go to Program.cs file -->
//there are many ways how you can do Authentification, but also we can keep it as default

```C#
//Here we make our Scheme in order to have JWT
builder.Services.AddAuthentication(options =>{
options.DefaultAuthenticateScheme =//there are many ways how you can do Authentification, here we will use more accurate way, but also we can keep it as default, lot of developers keep it as default

//here we can fill out the settings in case you want to change them
options.DefaultChallengeScheme =
options.DefaultForbidScheme =
options.DefaultScheme =
options.DefaultSignInScheme =
options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;  //this will set all defaults for us, all Schemes above
}).AddJwtBearer(options=>{ //Here we adding JWT
    options.TokenValidationParameters = new TokenValidationParameters(){  //TokenValidationParameters comes with JwtBearer, <-- downloaded Nuget Package
    ValidateIssuer = true,
    ValidIssuer = builder.Configuration["JWT:Issuer"],  //Issuer is a Server URL from appsettings.json, assign values from appsettings.json to the variablesappsettings.json
    ValidateAudience = true,
    ValidAudience = builder.Configuration["JWT:Audience"],   //Audience is the Users , who use our App, assign values from appsettings.json to the variables
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))   //SigningKey must be hidden !!!!!, it is Secret key. Assign values from appsettings.json to the variables. Here we use a form of encryption --> we convert data from appsettings.json file String --> to Bytes
    };
});
```

- Add JWT object in appsettings.json file

```C#
  "JWT": {
    "Issuer": "http//localhost:5113",   //needed when deploy our App in the Web, Issuer is a server
    "Audience": "http//localhost:5113",  //needed when deploy our App in the Web, Audience is the Users , who use our App
    "SigningKey": "ndandaij44fafefi4ovfvfgthzc35vnvs67kidxvse36ilors"  //is Secret key, must be hidden, because people can issuing their own tokens

    //SigningKey - must be long enough othewise it will show an error
```

- Add Addition code in Program.cs file

```C#

//these 2 line needed for Identity Framework Core (LogIn ,LogOut,SignIn, etc.)
app.UseAuthentication();
app.UseAuthorization();
```

- AFTER That, ALL THE FILES AND CODES for IDENTITY FRAMEWORK CORE (for LogIn,SigUp, Passwords, etc) --> we need to add new changes to DB and update the DB

```C#
dotnet ef migrations add (AnyMigrationsName) //to add changes to database
 dotnet ef migrations add Identity


dotnet ef database update  //to update database
```

# Registration

- We use UserManager (to get the user email from Registration Form and post it to Database)
- Usermanager will take care of everything and we don't have to do much, because UserManager is going to provide us this method below-->

```C#
//here we will pass the user and password within this method
//and it will hash & salted for us and it is going to put it to the DB
//password will be hashed & Salted (will look like this--> "AH378RSFGRHWG876553HHRYHR5784")
CreateAsync( user, "password")
```

- we hash things because if somebody will look into our Database and we don't want them to see the Password in clear text and words, that the user created. It will be difficult to get the passwords if ome one will breaks to your DB

To make User LogIn, SignUp, etc. we need to create separate Controller for Account --> SEE AccountController.cs and RegisterDto.cs files

- Before we can create any Users in DB, It has to be atleast one Role in DB, we need to add Roles--> in ApplicationDBContext.cs file --> otherwise it is going to show an Error. Because each user should have a role. Before we will try to Log someone in --> it supposed to be atleast 1 Role

- We going to create a User Role and Admin Role in Data/ApplicationDBContect.cs file

```C#
 //override onc + Tab , <--to have a taplate
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        List<IdentityRole> roles = new List<IdentityRole>(){     //Creating Identity Roles

        new IdentityRole(){
            Name = "Admin",
            NormalizedName = "ADMIN"   //NormalizedName means it is Capitalised
        },
        new IdentityRole(){
            Name = "User",
            NormalizedName = "USER"   //NormalizedName means it is Capitalised
        },
        };


        builder.Entity<IdentityRole>().HasData(roles);    //Adding roles to IdentityRole
    }
```

- Then we need to run command below to add User and Admin roles to the Database , into --> AspNetRoles table, otherwise it is going to show an Error.Register won't work. Because each user should have a role

```C#
dotnet ef migrations add (AnyMigrationsName) //to add changes to database
 dotnet ef migrations add SeedRole


dotnet ef database update  //to update database
```

# Claims Vs. Roles

Roles:

All we needed for app are User and Admin. You can easily add more complicated roles by inserting them into role table and adding annotations to controllers to prevent people from accessing certain endpoints. You could also add logic into controllers via RoleManager for more complicated scenarios

- Roles are more generic and broad (old school)
- if our App become more complex, for exapmple if we have 20 different roles and each time when we need to get the Role --> we need to get it from DB and that is limitation with Roles. To get the Role we have to hit the Database and DB should respond

This is wnhen Claims come in ...

Claims:

- Claims are everything, like a tag associated with the User
- Claims don't require DB and they are very flexible (new shool)
- Claims don't use DB and give much more flexibility (this makes Claims much better than Roles)
- Microsoft has moved away from Roles

#### Claims path

- Claims is almost like Roles --> Key -valie pairs of things that are going describe what the User does and what he can't do
- when User Submits his LogIn Form with Username , email etc. when LogIn--> it sends JWT to the Server
- And if LogIn data is correct --> User is Authenticated --> then We get the access to all the values in the DB related to this User, that we have created such as (User info(as example) --> username, email, is User loggedIn, etc.) --> (See Service/TokenService.cs line 35) <-- set User values that we have access to
- And all these data will be associated with this User
- We will have an object with all these User's data --> (See Service/TokenService.cs line 34) <--indicate the User prperties
- And we can use this object --> use this HttpUser.Context all throughout the app as long as the User is LogedIn. Object is created -->(See Service/TokenService.cs line 35)
- We get all User data once when User LogIn we don't need to hit the server every time

The concept of the ClaimPrincipal is almost like a valet, like Authentication valet <-- it is an object that holds all information about this User

- In our case we create ClaimPrincipal object with values that we have created in --> Service/TokenService.cs line 35, This values we can use to identify the User and express what the User can and can't do withing your app. Very similar to the Role( in Data/ApplicationDBContext.cs file)--> but more flexible

# Token Service

- we already written the code to handle validation of JWT
- Identity handles the rest! (validation)
- We can't generate JWT yet, we need to write code to handel generation of JWT
- We create the tokken! (generation)
- there are many ways to do this, but it easier to write your own service to create tokkens outside of Identity, there are tools that allow to do that

Creating Service to create tokkens outside of Identity-->

- Service is different from Repository, Repository is for DB calls but Service can be anything else, any type of abstraction.
- Services and Repositories use Interfaces by inhereting them

- Then we Create ITokenService Interface
- Create TokenService.cs --> in Service/ TokenService.cs
- TokkenService inherits --> : ITokenService
- Then in Program.cs --> we need to add/ register Dependency Injection (line 83)

```C#
builder.Services.AddScoped<ITokenService,TokenService>();
```

- Then in AccountController.cs --> when we Create new user --> when User was created Successfully --> instead of returnig message (--> See AccountController.cs line 76)

```C#
if(roleResult.Succeeded){
    return Ok("User created"); //returning this message, also we can return a token instead if message
}
```

how the app validate the token? The validation code is in the Program.cs. One way you can check what's wrong is looking at the response token and it will tell you the error inside the token. Very strange but it works great once you get used to it.

It is better to return userName, email and token,
We want return new User's --> userName and email address and token (properties that we created in Claims <--See TokenService.cs line 35 )

- Therefore we need to create new DTO (Form which we will return its props, in case of Successfully created User)
- Create NewUserDto.cs file in Dtos/Account

- insert ITokenService to AccountController.cs (line 34) , to have access to token in AccountController.cs file

- After Registration of new User --> received JWT Token we can decode Here --> to see if it is corectly working
  [JWT Decode](https://jwt.io/)

# Login

- We need to find the user --> using User Manager (used to find User)

```C#
_userManager.User.FirstOrDefaultAsync();
```

- Check password if it is correct--> using Signin Manager (used to check the password)

```C#
_signinManager.CheckPasswordSignInAsync()
```

- First we create LoginDto in --> Dtos/Account/LoginDto.cs (Form that will be used to LogIn )
- In AccountController.cs write Login POST Method

#### Adding [Authorize] atribute to back-end Swager Page and Test it

- Adding code below into your Program.cs file --> Swagger will have JWT built into it and we can test [Authorize] attribute

This code below will add Authorize button to Swagger page --> it will allow to add [Authorize] attribute to any action method --> then only loggedIn users can have access to those action methods

- Add [Authorize] attribute to any action methods in ItemControlles.cs

[Youtube Video](https://www.youtube.com/watch?v=WkFHTISvO4Q&list=PL82C6-O4XrHfrGOCPmKmwTO7M0avXyQKc&index=28)

```C#
//Swagger .NET Core Web API JWT Setup

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
```

- add this code above in Program.cs file after line 21

```C#
builder.Services.AddSwaggerGen();
```

# Many to many relationship

- many to many relationship are great for things like:
  - Favorites
  - Likes
  - Invites
  - Stock Portfolio

We want the User to be able to add infinite combinations of Items to their portfolio and we want other users to be ableto add items as well, so we need literally endless combinations of Items

One to many relationship has its limitations. One too many is bad because you can have only one Item as the parent so wouldn't be able to have the infinite combinations like we want (--> See one-to-many.png file in the Root )

To do many to many relationship --> we Must create join table, which will link Item table and User table together (--> See many-to-many.png file in the Root).

We must create Join table to link different tables together, Don't make many to many relationship without Join table (--> See do-not-do-thi.png file in the Root)
