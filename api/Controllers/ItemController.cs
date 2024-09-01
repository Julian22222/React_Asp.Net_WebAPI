using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;   //Can you Task with async await , and to use Task
using Microsoft.EntityFrameworkCore;  //to use ToListAsync method, SaveChangesAsync(), FindAsync(id); and other asyn methods
using api.Models;
using Microsoft.AspNetCore.Mvc;  // allow to use ControllerBase
using api.Repository;
using api.Interfaces;  // to use Interfaces
using api.Data;    
using api.Mappers;
using api.Dtos.Item;  //to use CreateItemRequestDto

namespace api.Controllers;

// [Route("api/[controller]")]
[Route("api/items")]   //<-- all the Routes will start with this (For Item)
[ApiController]  //this attribute indicates (tells to compilator) that we are going to use API Controller in this Class
public class ItemController : ControllerBase  //<--Inherit from ControllerBase , it allows to bring these attributes -->[Route("api/items")][ApiController].  ControllerBase - is used for WEB API only
{

    private readonly ApplicationDBContext _context;  //for DB always use --> private readonly , this provide more security against people modefying something in DB
    private readonly IItemRepository _iitemRepository;  //create Interface variable,

    //constructor
    public ItemController(ApplicationDBContext context, Interfaces.IItemRepository iitemRepository){
        _context = context;
        _iitemRepository = iitemRepository;
    }



    //This Method have the Route --> [Route("api/items")] 
    [HttpGet]
    public async Task<IActionResult> GetAll(){

        var data = await _iitemRepository.GetAllItems();  // use interface(which is linked with ItemRepository) and its method to get the data from DB,  Use ItemRepository methods

        return Ok(data);  //returning 200 Status Code and the data from DB
    }





    [HttpGet("error")]  //Error hadling for HttpGet request, URL -->[Route("api/items")] + /error 
    public async Task<IActionResult> Error() {
        throw new FileNotFoundException();   //will return full error details (All error response in Sqagger -->in Response body), In Production use of API, Client doesn't need to see all the error details.

        //To make error more structured and shorten (less details, only important info will be shown in Sqagger--> in Response body) --> we use Program.cs line 41 --> (write this code -->app.UseExceptionHandler("/ErrorHandling/ProcessError");)  and Create ErrorHandlingController.cs file
    
        // Then We need to adjust this error hadling --> For Development production (show all error details). For Production environment (show only limited eror details for Client)
        //It can be adjusted in ErrorHandlingController.cs file (line 32)
    }



    //End-point for this method will be --> [Route("api/items")] + /{id}
    // This Method will create new route for each item -->[Route("api/items")] + /{id}  //<--takes a variable
    [HttpGet("{id}")]  //get a id variable, this id is transfered to --> ([FromRoute] int id) parametr --> .NET extraact "{id}" from string out turning it to --> int id. .Net do the Model binding
    public async Task<IActionResult> GetById([FromRoute] int id){  //<--getting id from the URL 
        
        var item = await _iitemRepository.GetItemById(id);    //Use ItemRepository methods

        if(item == null){  //if item is not existing
        return NotFound();  //<-- is a form of IActionResult, don't need to write the status code and etc.
        }

        // return Ok(item); //<-- if Item was found, return an item, full object of item properties

        return Ok(item.ToItemDto());  //<-- invoke static ToitemDto method from Mappers folder/ ItemMappers file. Will return only properies from ToitemDto method
        ////return an object in --> ItemDto Format, using ToItemDto method
    
    }




    [HttpPost]

    //will receive the data in format of --> ItemDto Model
    //CreateItemRequestDto <--Data type
    //[FromBody]  <-- annotations, we take the data from "body" to POST, For POST Method we don't need to send an Id, Entity Framework (EF) will create Id automatically
    public async Task<IActionResult> Create([FromBody] CreateItemRequestDto ItemDto){  //we need [FromBody], because our Data will be in form of JSON, we not passing the data through URL, we passing the data in the--> body of HTTP. //CreateItemRequestDto will be a template Form to fill to Add new object to DB, will appear in Swagger Schemas
    //Create Request portion of our DTO, user will need to submit only data from DTO , but not to fill all properties from Item Class
    //for a POST method we create another DTO --> CreateItemRequestDto, it will help to trim out not needed properties to POST (No need -> Id, and comments from Item Model)

    var itemModel = ItemDto.ToItemFromCreateDTO();  //Convert it to specfic dat type using ToItemFromCreateDTO Method

    await _iitemRepository.CreateNewItem(itemModel);   //Use ItemRepository methods

    return CreatedAtAction(nameof(GetById), new {id = itemModel.Id}, itemModel.ToItemDto());  //return an object in --> ItemDto Format, using ToItemDto method

    //CreatedAtAction --> will execute GetByID method and pass id (new {id = itemModel.Id}) to this method and after that it will return --> itemModel.ToItemDto() the data in this form

    }




    [HttpPut]
    [Route("{id}")]  //<-- we need to specify an id (or some type of identifier), we will find an object by Id chich needs to be changed
    //[FromRoute] , [FromBody]  <-- are annotations, these are the data which we need for PUT Method. 
    //[FromBody] --> We POST data in the body,we put JSON in the body. 
    //[FromRoute] int id --> we get an id, in data type -> int, with the name - id
    //UpdateItemRequestDto --> new DTO, new created Model, with the name --> updateDto (where will be all changes in POST body)
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateItemRequestDto updateDto){  //UpdateItemRequestDto will be a template Form to fill to Update the object, will appear in Swagger Schemas

        var itemModel = await _iitemRepository.UpdateItem(id, updateDto);  //Use ItemRepository methods

        if (itemModel == null){ 
            return NotFound();
        }

        return Ok(itemModel.ToItemDto());  //return an object in --> ItemDto Format, using ToItemDto method

    }






    [HttpDelete]
    [Route("{id}")]  //<-- we use Route id, from URL to find needed object and then we can delete it
    public async Task<IActionResult> Delete([FromRoute] int id){   //<--getting id from the URL 
        
        var itemModel = await _iitemRepository.DeleteItem(id);   //Use ItemRepository methods


        if (itemModel == null){  //if no object found 
            return NotFound();
        }

        return NoContent();   //<-- used for Delete object, return status 204, Success
    }


  

}
