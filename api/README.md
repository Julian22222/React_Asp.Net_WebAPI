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

SEE --> layout.tldr file

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
```

- Then to use this packages in our Appwe need to write some code in Program.cs file (line 19)

```C#
//To Use NewtonsoftJson extensions, we need to write this code -->
builder.Services.AddControllers().AddNewtonsoftJson(options =>{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;  // write this code to prevent object cycles
});
```

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

```
