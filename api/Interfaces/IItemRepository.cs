using api.Models;
using api.Dtos.Item;
using api.Helpers;

namespace api.Interfaces
{
    public interface IItemRepository
    {

         //This Interface has the same Methods and properties as ItemRepository Class in Repository folder

        //Interfaces allow as to plug in this code tp other places and allow us to abstract our code away
        //Interfaces can be pluged in to our Repository files, and quikly implement the interface from Interface file
        //The Class which will inherit this Interface MUST have the same methods and arguments


        // Task<List<Item>> GetAllItems();  //Method without a query

        Task<List<Item>> GetAllItems(QueryObject query);  //Method with the query

        Task<Item?> GetItemById(int id);  //Item? <-- because this method uses - FirstOrDefault --> an it CAN BE NULL, if it is not find something it will return null
    

        Task<Item> CreateNewItem (Item itemModel);   // for POST method  

        Task<Item?> UpdateItem (int id, UpdateItemRequestDto itemDto);   //Item? <-- Can have an Item or CAN BE NULL, if it is not find something it will return null
    
        Task<Item?> DeleteItem(int id);    //Item? <-- Can have an Item or CAN BE NULL, if it is not find something it will return null

        Task<bool> IsItemExist(int id);   // by this method we can chec if the Item exist, when we add the comment to this Item
    }
}