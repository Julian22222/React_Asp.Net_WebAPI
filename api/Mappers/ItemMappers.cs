using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;  // to use Item Model
using api.Dtos.Item;  //to use ItemDto Model

namespace api.Mappers
{
    public static class ItemMappers  //static file
    {   

         //Here We Convert one Item Data type to another Item Data type


        //different Methods in this class thit we can use
        //This is for GET Request (For Item)
        // ItemDto <-- we returning this type of data
        // ToitemDto <-- the name of the method
        public static ItemDto ToItemDto(this Item item){ //static method, receives Item(Data type and we give a name --> item)

            return new ItemDto(){  //we returning new ItemDto
                Id = item.Id,
                Name = item.Name,
                Type = item.Type,
                Qty = item.Qty,
                Price = item.Price,
                Description = item.Description,
                Comments = item.Comments.Select(x => x.ToCommentDto()).ToList(),  // Select(x => x.ToCommentDto <-- we are mapping all comments and convert then to ToCommentDto Format
                //trmmed out in ItemDto Class first and then --> we use ItemDto class here
                //trimed out property from Item Model, because we don't want to use it here
            };
        }


        //this is for POST Request
        // CreateItemRequestDto --> receives data type
        //when we Post something to DB it MUST to be in that data type what we use in that DB table ,where we POST (in our case MUST be --> Item Model). Our Database Items table has item Model data type (We can POST only Item Model to the DB Items table, It CAN"T BE IN FORMAT OF DTO --> not a trmmed out Models)
        //return Item data type
        //ToItemFromCreateDTO - name of this method
        //CreateItemRequestDto <-- receives this data type (created Model)
        //ItemDto the name of received data, can be any name
        public static Item ToItemFromCreateDTO (this CreateItemRequestDto ItemDto){

            return new Item(){
                //don't need to assign Id
                Name = ItemDto.Name,
                Type = ItemDto.Type,
                Qty =  ItemDto.Qty,
                Description = ItemDto.Description,
                Price = ItemDto.Price,
                // Comments = 
            };
        }


    }
}