# Different Classes (DTO) & How to use them

# Item Class

- Located in Model/Item.cs

```C#
//It is Database Format of Item, that we keep in the DB

public int Id { get; set;}

public string Name { get; set; } = string.Empty;  //<--string.Empty == "";

public string Type { get; set; }

public string Description { get; set; } = "";

public int Qty { get; set; }

public decimal Price { get; set; }

//one to many relationship, we use Comment Model in here, That you can leave many comments under one Item
//Here we are linking two DB tables
public List<Comment> Comments { get; set; } = new List<Comment>();
```

# ItemDto Class

- Located in Dtos/Item/ItemDto.cs
- Used in GET Requests
- Used to Show user Item in this Format, show these properties

```C#
public int Id { get; set;}

public string Name { get; set; } = string.Empty;  //<--string.Empty == "";

public string Type { get; set; } = "";

public string Description { get; set; } = "";

public int Qty { get; set; } = 0;

public decimal Price { get; set; }

public List<CommentDto> Comments { get; set; }  //Show the comments in special Format

```

# CreateItemRequestDto Class

- Located in Dtos/Item/CreateItemRequestDto.cs
- Used in Form to fill in Front-End /or Swagger --> then we convert it to Item Format to Add this Class to Database

```C#

public string Name { get; set; } = string.Empty;  //<--string.Empty == "";

public string Type { get; set; }

public string Description { get; set; } = "";

public int Qty { get; set; } = 0;

public decimal Price { get; set; }

//When Adding new Item to the DB from the Form, we don't need Id and <Comment> Comments properties
```

# UpdateItemRequestDto Class

- Located in Dtos/Item/UpdateItemRequestDto.cs
- Used in Form to fill in Front-End /or Swagger

```C#
public string Name { get; set; } = string.Empty;  //<--string.Empty == "";

public string Type { get; set; }

public string Description { get; set; } = "";

public int Qty { get; set; } = 0;

public decimal Price { get; set; }

//When Updating the Item in the DB from the Form, we don't need Id and <Comment> Comments properties
```

---

---

# Comment

- Located in Model/Comment.cs

```C#
//It is Database Format of Comment, that we keep in the DB

public int Id { get; set; }   //Primary Key

public string Title { get; set; } = string.Empty;

public string Content { get; set; } = string.Empty;

public DateTime CreatedOn { get; set; } = DateTime.Now;

public int? ItemId { get; set; }  //foreign key of Items Table , each comment should be referenced to some Item

//Linking two DB tables
public Item? Item { get; set; }    //field is optional
```

# CommentDto Class

- Located in Dtos/Comment/CommentDto.cs
- Used in GET Requests
- Used to Show user Commnet in this Format, show these properties

```C#
public int Id { get; set; }    //Primary Key

public string Title { get; set; } = string.Empty;

public string Content { get; set; } = string.Empty;

public DateTime CreatedOn { get; set; } = DateTime.Now;

public int? ItemId { get; set; }  //foreign key of Items Table


//Don't need in this Class
//  public Item? Item { get; set; }
```

# CreateCommentDto Class

- Located in Dtos/Comment/CreateCommentDto.cs
- Used in Form to fill in Front-End /or Swagger --> then we convert it to Comment Format to Add this Class to Database

```C#
public string Title { get; set; } = string.Empty;

public string Content { get; set; } = string.Empty;

//When Adding new Comment to the DB from the Form, we don't need Comment Id and other  properties
```

# UpdateCommentRequestDto Class

- Located in Dtos/Comment/UpdateCommentRequestDto.cs
- Used in Form to fill in Front-End /or Swagger

```C#
public string Title { get; set; } = string.Empty;

public string Content { get; set; } = string.Empty;
```
