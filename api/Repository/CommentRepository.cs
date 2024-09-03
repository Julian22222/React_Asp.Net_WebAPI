using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;   //Can you Task with async await , and to use Task
using Microsoft.EntityFrameworkCore;  //to use ToListAsync method, SaveChangesAsync(), FindAsync(id); and other asyn methods
using api.Models;
using api.Interfaces;  //To use ICommentRepository
using api.Data;


namespace api.Repository;

//This Class must Implementing all interface methods (From Interfaces/ICommentRepository) !!!!!!!!!!!!
//This Class inherits ICommentRepository Interface, therefore this Class MUST have the same methods and take the same arguments as --> ICommentRepository Interface

public class CommentRepository : ICommentRepository   // inherit from Interface
{

    private readonly ApplicationDBContext _context;   //assign a variable

    //ctor + tab ,   <--constructor
    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }


    public async Task<List<Comment>> GetAllComments(){
        return await _context.Comments.ToListAsync();
    }


    //Comment? <-- Can have a Comment or CAN BE NULL, if it is not find something it will return null
    public async Task<Comment?> GetCommentById(int id){
        return await _context.Comments.FindAsync(id);  //Find needed Comment, received object is in Comment Model Format
    } 



    public async Task<Comment> CreateComment(Comment commentModel){

        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();

        return commentModel;
    }




    public async Task<Comment?> UpdateComment(int commentId, Comment commentModel){

        var existingComment = await _context.Comments.FindAsync(commentId);   //checking is the comment exist in DB  
    
        if (existingComment == null){
            return null;
        }


        //Update the comment
        existingComment.Title = commentModel.Title;
        existingComment.Content = commentModel.Content;


        await _context.SaveChangesAsync();   //Save changes in DB
        return existingComment;   //return Chanhged Comment
    }




    public async Task<Comment?> DeleteComment(int commentId){

        var commentModel = await _context.Comments.FirstOrDefaultAsync(x=> x.Id == commentId);   //checking is the comment exist in DB  

        if(commentModel == null){
            return null;
        }

        _context.Comments.Remove(commentModel);   //don't use await with Remove Method, Remove dosn't have an await method
        await _context.SaveChangesAsync();

        return commentModel;
    }


}
