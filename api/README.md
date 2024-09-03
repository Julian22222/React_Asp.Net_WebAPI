[Web API Docs with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-8.0&WT.mc_id=dotnet-35129-website&tabs=visual-studio-code)

# Swagger Documentation

- Is called Swagger or Swagger Interface
- It is a library that interacts with Interface for special visualisation of API
- Show all your API routes

- Heading in Swagger interface is taken from Controller Names (Comment or Item) --> From CommentController , ItemController

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

### DTO is used --> when we want to modefy some properies in already existing Class--> we create similar class and delete some properties that we don't need (Work together with Auto Mapper)

- it is creating new object but with modified properies (example: Item and ItemDto)
- We create Dtos folder and put all new modefied Models files inside that folder
- DTO Models are used only to show some Data to the Clinet, we can't send DTO to DB
- if we want to show some details but we don't need full object of all properties
- For example we don't want to receive the Comments for an Item, then we create the same Item object but without Comments property
- DTO and Mapper work together

### Mapper (work together with DTO)

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
```

- Then to use this packages in our Appwe need to write some code in Program.cs file (line 19)

```C#
//To Use NewtonsoftJson extensions, we need to write this code -->
builder.Services.AddControllers().AddNewtonsoftJson(options =>{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;  // write this code to prevent object cycles
});
```
