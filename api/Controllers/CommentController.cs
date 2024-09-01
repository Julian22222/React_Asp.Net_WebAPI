using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;   //Can you Task with async await , and to use Task
using Microsoft.EntityFrameworkCore;  //to use ToListAsync method, SaveChangesAsync(), FindAsync(id); and other asyn methods
using Microsoft.AspNetCore.Mvc;  // allow to use ControllerBase
using api.Models;
using api.Mappers;  //To use CommentMapper
using api.Interfaces;  // to use ICommentRepository

namespace api.Controllers;

[Route("api/comments")]   //<-- all the Routes will start with this (For Comments)
[ApiController]   //this attribute indicates (tells to compilator) that we are going to use API Controller in this Class
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepo;


    public CommentController(ICommentRepository commentRepo)
    {
        _commentRepo = commentRepo;
    }



    [HttpGet]
    public async Task<IActionResult> GetAll(){
        var comments = await _commentRepo.GetAllComments();  //invoke GetAllComments Method from CommentRepository
    
        var commentDto =  comments.Select(x=>x.ToCommentDto());  //  ToCommentDto Static Method --> in CommentMapper
        return Ok(commentDto);
    }

}
