using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;   //Can you Task with async await , and to use Task
using Microsoft.EntityFrameworkCore;  //to use ToListAsync method, SaveChangesAsync(), FindAsync(id); and other asyn methods
using api.Models;
using Microsoft.AspNetCore.Mvc;
using api.Repository;
using api.Data;
using api.Mappers;
using api.Dtos.Item;  //to use CreateItemRequestDto
using api.Interfaces;


namespace api.Repository;

//This Class must Implementing all interface methods (From Interfaces/IItemRepository) !!!!!!!!!!!!
//This Class inherits IItemRepository Interface, therefore this Class MUST have the same methods and take the same arguments as --> IItemRepository Interface

public class ItemRepository : IItemRepository  //inherit from Interface
{

    private readonly ApplicationDBContext _context;  //for DB always use --> private readonly , this provide more security against people modefying something in DB
    


    //ctor +tab --> constructor
    public ItemRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    


    //return List with data type--> Item, GetAll - is a name of this method
    public async Task<List<Item>> GetAllItems(){

        // ToList(); - will help this command to be executed, to go to SQL Server and get what we asked for
        // var items = await _context.Items.ToListAsync();  //<-- this line will return all Object of Item , with all properties

        //Select is .NET version of a .map in JavaScript, we map over the List of object and passing each object to --> static ToitemDto method
        // var items = await _context.Items.ToListAsync();   //<-- this line will return an object in Item Model Format
        
        // var newItem = items.Select(x => x.ToItemDto()); // Item Model Format (data type) we convert to ItemDto (data type)   using --> Mappers/ItemMappers.cs file

        return await _context.Items.ToListAsync(); 
    }




    public async Task<Item?> GetItemById(int id){
        return await _context.Items.FindAsync(id);    //Find needed item, received object is in Item Model
    }





    public async Task<Item> CreateNewItem(Item itemModel){
        await _context.Items.AddAsync(itemModel);
        await _context.SaveChangesAsync();

        return itemModel;
    }



    public async Task<Item?> UpdateItem(int id, UpdateItemRequestDto itemDto){  //itemDto data from posted body from client

    var existingItem = await _context.Items.FirstOrDefaultAsync(x => x.Id == id); //find needed Item in Item Format
    // var existingItem = await _context.Items.SelectAsync(x => x.Id == id).FirstOrDefaultAsync();  //<--the same
    //var existingItem = await _context.Items.FindAsync(id);   //<-- the same



    if(existingItem == null){  //if Item not found return null
        return null;
    }


         //Update the Object, here we modify our object straight away , asign to received item from BD -->with new variables from posted body from client
        existingItem.Name = itemDto.Name;
        existingItem.Type = itemDto.Type;
        existingItem.Description = itemDto.Description;
        existingItem.Qty = itemDto.Qty;
        existingItem.Price = itemDto.Price;

        //Save updated object in the DB
        await _context.SaveChangesAsync();

        return existingItem; //return updated object
    }




    public async Task<Item?> DeleteItem(int id){
        
    var itemModel = await _context.Items.FindAsync(id);   //<-- find an object by id from DB
     //var itemModel = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);  /<--the same


    if(itemModel == null){
        return null;
    }

    _context.Items.Remove(itemModel);   //<-- Remove is NOT Asynchrones function, Don't need to add --> await in front of this line
    await _context.SaveChangesAsync();

    return itemModel;
    }
}
