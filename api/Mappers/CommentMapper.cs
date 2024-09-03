using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;  //to use CommentDto Class
using api.Models;  // To use Item Class


namespace api.Mappers;

public static class CommentMapper   //static file
{

    //Here We Convert one Component Data type to another Comment Data type

    public static CommentDto ToCommentDto (this Comment commentModel){   //static method  //CommentDto <--data type, ToCommentDto <--method name

    return new CommentDto(){   
        Id = commentModel.Id,
        Title = commentModel.Title,
        Content = commentModel.Content,
        CreatedOn = commentModel.CreatedOn,
        ItemId = commentModel.ItemId,
         //trimmed out in CommentDto Class first and then we use CommentDto class here
         //trimed out property from Comment Model, because we don't want to use it here --> public Item? Item { get; set; }    <--(this property we don't need)
    };
    }




    public static Comment ToCommentFromCreate (this CreateCommentDto commentDto, int itemId){   //static method  //CreateCommentDto <--data type, commentDto <--method name,  int itemId <-- foreign key for Item
    //From  CommentController.cs --> we use this method -->//commentDto.ToCommentFromCreate(ItemId); /<--CreateCommentDto comes from CreateCommentDto commentDto, int itemId comes from --> ItemId
    
    return new Comment(){   
        //Id <-- we don't neeed it is created automatically by EntityFramework Core
        Title = commentDto.Title,
        Content = commentDto.Content,
        ItemId = itemId   //<-- asign a foreign key 
    };
    }





    public static Comment ToCommentFromUpdate (this UpdateCommentRequestDto updateDto){   //static method  //UpdateCommentRequestDto <--data type, updateDto <--method name
    
    //Convert UpdateCommentRequestDto data type to Comment data type
    return new Comment(){   
        Title = updateDto.Title,
        Content = updateDto.Content,
    };
    }
}
