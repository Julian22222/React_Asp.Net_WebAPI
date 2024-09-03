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


    public async Task<Comment?> GetCommentById(int id){
        return await _context.Comments.FindAsync(id);  //Find needed Comment, received object is in Comment Model Format
    } 



    public async Task<Comment> CreateComment(Comment commentModel){

        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();

        return commentModel;
    }



}
