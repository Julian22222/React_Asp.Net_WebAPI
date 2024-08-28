using api.Models;
using Microsoft.AspNetCore.Mvc;
using api.Repository;

namespace api.Controllers;

public class ItemController : Controller
{

    private readonly ItemRepository _itemRepository;
    

    //constructor
    public ItemController(ItemRepository itemRepository){
        _itemRepository = itemRepository;   
    }


    public List<Item> GetAllItems(){

        return _itemRepository.GetAllItems();
    }


     public Item GetItem(int id){

        return _itemRepository.GetItemById(id);
    }

      public List<Item> SearchBooks(string title){

        return _itemRepository.SearchItem(title);
    }

}
