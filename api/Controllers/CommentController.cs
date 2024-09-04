using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;   //Can you Task with async await , and to use Task
using Microsoft.EntityFrameworkCore;  //to use ToListAsync method, SaveChangesAsync(), FindAsync(id); and other asyn methods
using Microsoft.AspNetCore.Mvc;  // allow to use ControllerBase
using api.Models;
using api.Mappers;  //To use CommentMapper
using api.Interfaces;  // to use ICommentRepository
using api.Repository;
using api.Data;    
using api.Mappers;
using api.Dtos.Item;  //to use CreateItemRequestDto
using api.Dtos.Comment;  //to use CreateCommentDto

namespace api.Controllers;

[Route("api/comments")]   //<-- all the Routes will start with this (For Comments)
[ApiController]   //this attribute indicates (tells to compilator) that we are going to use API Controller in this Class
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _icommentRepo;  ////import CommentRepository Class (to use its methods), through ICommentRepository Interface
    private readonly IItemRepository _itemRepository;  //import temRepository Class (to use its methods), through IItemRepository Interface


    public CommentController(ICommentRepository icommentRepo, IItemRepository itemRepository)
    {
        _icommentRepo = icommentRepo;
        _itemRepository = itemRepository;
    }



    [HttpGet]
    public async Task<IActionResult> GetAll(){


        if(!ModelState.IsValid){     //Here we perform all Data Validation from DTO Class, ModelState is coming from /inheriting from ControllerBase 
            return BadRequest(ModelState);  
        }


        var comments = await _icommentRepo.GetAllComments();  //invoke GetAllComments Method from CommentRepository
    
        var commentDto =  comments.Select(x=>x.ToCommentDto());  // Convert Comment data type to specfic data type using ToCommentDto Method ToCommentDto Static Method --> in CommentMapper
        return Ok(commentDto);
    }




     // This Method will create new route for each comment -->[Route("api/comments")] + /{id}  //<--takes a variable
    [HttpGet("{id:int}")]  //Id must be an intager (whole number), This is Data Validation
    public async Task<IActionResult> GetById([FromRoute] int id){  //get a id variable from URL, this id is transfered to --> ([FromRoute] int id) parametr --> .NET extraact "{id}" from string out turning it to --> int id. .Net do the Model binding


        if(!ModelState.IsValid){     //Here we perform all Data Validation from DTO Class, ModelState is coming from /inheriting from ControllerBase 
            return BadRequest(ModelState);  
        }


        var comment = await _icommentRepo.GetCommentById(id);  // Use CommentRepository method

        if(comment == null){  //if Comment is not existing
            return NotFound();
        }
        
        return Ok(comment.ToCommentDto());   //Convert Comment data type to specfic data type using ToCommentDto Method, invoke static ToCommentDto method from Mappers folder/ CommentMapper file. Will return only properies from ToCommentDto method
         ////return an object in --> CommentDto Format, using ToCommentDto method
    }




    //To add a comment, each comment must have Foreign key (to be referenced to one of Item)
    //If we create a comment it Has to have a Parent (to have Foreign Key)
    [HttpPost("{ItemId:int}")]   //ItemId is a Foreign Key -->to be reference to one of the Item), ItemId must be an intager
    //[FromBody]  <-- annotations, we take the data from "body" to POST, For POST Method we don't need to send an Id (Primary Key for Comment), Entity Framework (EF) will create Id automatically
    //[FromRoute] int ItemId <-- will take an Id from URL (It is Foreign Key - to be reference to one of the Item), 
    //CreateCommentDto <-- data type(created DTO Class), commentDto <-- name of this data
    public async Task<IActionResult> Create([FromRoute] int ItemId, CreateCommentDto commentDto){   // CreateCommentDto commentDto --> is a body to post a data


        if(!ModelState.IsValid){     //Here we perform all Data Validation from DTO Class, ModelState is coming from /inheriting from ControllerBase 
            return BadRequest(ModelState);  
        }


        if(!await _itemRepository.IsItemExist(ItemId)){  //If Item doesn't exist 
            return BadRequest("Item does not exist");
        }

        var commentModel = commentDto.ToCommentFromCreate(ItemId);   //Convert CreateCommentDto data type to specfic data type using ToCommentFromCreate Method
        //here we get a Item data
        await _icommentRepo.CreateComment(commentModel);

        return CreatedAtAction(nameof(GetById), new {id = commentModel.Id}, commentModel.ToCommentDto());   // passing {id = commentModel} <-- to be able to find actual comment,  commentModel.ToCommentDto() <-- return in Form that we want
    }



    [HttpPut]  //Update a Comment
    [Route("{id:int}")] //getting comment Id from the URL, id must be an intager
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto){  //[FromRoute] int id <-- getting comment id from the URL, [FromBody] UpdateCommentRequestDto updateDto  <-- update Json body
    

        if(!ModelState.IsValid){     //Here we perform all Data Validation from DTO Class, ModelState is coming from /inheriting from ControllerBase 
            return BadRequest(ModelState);  
        }


      var comment = await _icommentRepo.UpdateComment(id, updateDto.ToCommentFromUpdate());

      if(comment == null){   //if there is no comment with this id
        return NotFound("Comment not found");
      }

      return Ok(comment.ToCommentDto());   //Convert Comment data type to CommentDto data type
    }




    [HttpDelete]
    [Route("{id:int}")]  //getting comment Id from the URL, int must be an intager
    public async Task<ActionResult> Delete([FromRoute] int id){


        if(!ModelState.IsValid){     //Here we perform all Data Validation from DTO Class, ModelState is coming from /inheriting from ControllerBase 
            return BadRequest(ModelState);  
        }

        var commentModel = await _icommentRepo.DeleteComment(id);

        if(commentModel == null){ //if there is no comment with this id
            return NotFound("Comment does not exist");
        }

        return Ok(commentModel);  //We don't have to return commentModel, it can be empty --> return Ok();
    }



}
